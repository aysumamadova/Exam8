using Microsoft.AspNetCore.Identity;

namespace Exam8.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
