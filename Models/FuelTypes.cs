using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class FuelTypes
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Yakıt Türü")]
        public string Name { get; set; }
    }
}
