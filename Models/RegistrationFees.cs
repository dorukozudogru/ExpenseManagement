using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class RegistrationFees
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Yıl")]
        public int Year { get; set; }

        [DisplayName("Trafik Tescil Bedeli")]
        public double RegistrationFee { get; set; }
    }
}