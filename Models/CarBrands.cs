using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class CarBrands
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Marka Adı")]
        public string Name { get; set; }
    }
}
