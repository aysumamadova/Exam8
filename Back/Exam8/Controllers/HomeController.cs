using Exam8.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Exam8.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _conetxt;

        public HomeController(AppDbContext conetxt)
        {
            _conetxt = conetxt;
        }

        public IActionResult Index()
        {
            return View(_conetxt.Customers.ToList());
        }

    }
}
