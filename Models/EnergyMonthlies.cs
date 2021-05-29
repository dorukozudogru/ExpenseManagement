using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class EnergyMonthlies
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Yıl")]
        public int Year { get; set; }

        [Required]
        [DisplayName("Ay")]
        public byte? Month { get; set; }

        [NotMapped]
        [DisplayName("Ay")]
        public string MonthName { get; set; }

        [Required]
        [DisplayName("Üretilen Kw")]
        public double ProducedKw { get; set; }

        [Required]
        [DisplayName("Tüketilen Kw")]
        public double ConsumedKw { get; set; }

        [Required]
        [DisplayName("Dağıtım Bedeli")]
        public double DistributionFee { get; set; }

        [Required]
        [DisplayName("Fatura Tutarı (KDV Hariç)")]
        public double Amount { get; set; }

        [DisplayName("KDV")]
        public double TAX { get; set; }
    }
}
