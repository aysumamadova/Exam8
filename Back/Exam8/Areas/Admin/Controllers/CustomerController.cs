using Exam8.DAL;
using Exam8.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Exam8.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CustomerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(customer);
                Customer c = new Customer
                {
                    Name = customer.Name,
                    Info = customer.Info,
                    Work = customer.Work,
                    Img = uniqueFileName
                };

                _context.Add(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        private string ProcessUploadedFile(Customer customer)
        {
            string uniqueFileName = null;

            if (customer.Photo != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + customer.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    customer.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Customer customer = _context.Customers.Find(id);
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var c = await _context.Customers.FindAsync(id);
            var customer = new Customer()
            {
                Id = c.Id,
                Name = c.Name,
                Info = c.Info,
                Work = c.Work,
                Img = c.Img,
            };

            if (c == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (ModelState.IsValid)
            {
                var c = await _context.Customers.FindAsync(customer.Id);
                c.Name = customer.Name;
                c.Info = customer.Info;
                c.Work = customer.Work;
                if (customer.Photo != null)
                {
                    if (customer.Img != null)
                    {
                        string filePath = Path.Combine(_env.WebRootPath, "assets", "img", customer.Img);
                        System.IO.File.Delete(filePath);
                    }

                    c.Img = ProcessUploadedFile(customer);
                }
                _context.Update(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

    }
}
