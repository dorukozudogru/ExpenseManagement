using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class BankBranches
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Banka/Şube Adı")]
        public string Name { get; set; }
    }
}
