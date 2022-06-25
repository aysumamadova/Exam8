using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam8.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Img { get; set; }
        public string Work { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
