using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DisplayName("E-Posta")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Şifre")]
        public string Password { get; set; }

        [DisplayName("Beni Hatırla")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        [DisplayName("Aktif Mi?")]
        public bool IsActive { get; set; }
    }
}
