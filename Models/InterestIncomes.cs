using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class InterestIncomes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Yıl")]
        public int Year { get; set; }

        [Required]
        [DisplayName("Faiz Tipi")]
        public byte InterestType { get; set; }

        [NotMapped]
        [DisplayName("Faiz Tipi")]
        public string InterestTypeName { get; set; }

        public enum InterestTypeEnum
        {
            [Display(Name = "Gecelik")]
            OVERNIGHT_INTEREST = 0,
            [Display(Name = "Uzun Dönem")]
            LONG_TERM_INTEREST = 1,
        }

        [Required]
        [DisplayName("Tutar")]
        public double Amount { get; set; }
    }
}
