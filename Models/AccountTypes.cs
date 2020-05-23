using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class AccountTypes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Hesap Türü")]
        public string Name { get; set; }
    }
}
