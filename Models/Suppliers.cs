using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class Suppliers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Satıcı/Tedarikçi")]
        public string Name { get; set; }
    }
}
