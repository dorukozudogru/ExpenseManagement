using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace ExpenseManagement.Models
{
    public class AppIdentityUser : IdentityUser
    {
        //[DisplayName("Adı")]
        //public string Name { get; set; }
        //[DisplayName("Soyadı")]
        //public string Surname { get; set; }

        [DisplayName("Aktif Mi?")]
        public bool IsActive { get; set; }

        [DisplayName("Admin Mi?")]
        public bool IsAdmin { get; set; }
    }
}
