using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class PointOfSale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Yıl")]
        public int Year { get; set; }

        [Required]
        [DisplayName("Ay")]
        public byte Month { get; set; }

        [NotMapped]
        [DisplayName("Ay")]
        public string MonthName { get; set; }

        [Required]
        [ForeignKey(nameof(Sector))]
        [DisplayName("Sektör/İş Kolu Adı")]
        public int SectorId { get; set; }
        [DisplayName("Sektör/İş Kolu Adı")]
        public virtual Sectors Sector { get; set; }
        [NotMapped]
        public string SectorName { get; set; }

        [DisplayName("Tarih")]
        public DateTime? Date { get; set; }

        [Required]
        [ForeignKey(nameof(BankBranches))]
        [DisplayName("Banka/Şube Adı")]
        public int BankBranchId { get; set; }
        [DisplayName("Banka/Şube Adı")]
        public virtual BankBranches BankBranch { get; set; }

        [DisplayName("Tutar")]
        public double Amount { get; set; }

        [DisplayName("Tutar Dövizi")]
        public byte AmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Tutar Dövizi")]
        public string AmountCurrencyName { get; set; }
    }
}
