using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class Endorsements
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Sector))]
        public int SectorId { get; set; }
        public virtual Sectors Sector { get; set; }
        [NotMapped]
        [DisplayName("Sektör/İş Kolu Adı")]
        public string SectorName { get; set; }

        [Required]
        [DisplayName("Tutar")]
        public double Amount { get; set; }

        [NotMapped]
        [DisplayName("Toplam Tutar")]
        public double TotalAmount { get; set; }

        [Required]
        [DisplayName("Tutar Dövizi")]
        public byte AmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Tutar Dövizi")]
        public string AmountCurrencyName { get; set; }

        public enum AmountCurrencyEnum
        {
            [Display(Name = "₺")]
            TRY = 0,
            [Display(Name = "$")]
            USD = 1,
            [Display(Name = "€")]
            EUR = 2,
            [Display(Name = "£")]
            GBP = 3
        }

        [Required]
        [DisplayName("Ay")]
        public byte Month { get; set; }

        [NotMapped]
        [DisplayName("Ay")]
        public string MonthName { get; set; }

        [Required]
        [DisplayName("Yıl")]
        public int Year { get; set; }
    }
}
