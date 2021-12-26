using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class WashingIncome
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Yıl")]
        public int Year { get; set; }

        [Required]
        [DisplayName("Brüt Kâr")]
        public double GrossProfit { get; set; }

        [Required]
        [DisplayName("Masraf")]
        public double Cost { get; set; }

        [Required]
        [DisplayName("Net Kâr")]
        public double NetProfit { get; set; }
    }
}