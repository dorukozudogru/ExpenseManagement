using Microsoft.AspNetCore.Identity;
using System;
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

        [DisplayName("Son Oturum Açma Tarihi")]
        public DateTime? LastLoginDate { get; set; }

        [DisplayName("Muayene Tarihi Yaklaşan Araçlar Görüldü Tarihi")]
        public DateTime? FinishingInspectionClickDate { get; set; }

        [DisplayName("Bitiş Tarihi Yaklaşan Teminat Mektubu Görüldü Tarihi")]
        public DateTime? FinishingGuaranteeClickDate { get; set; }
    }
}
