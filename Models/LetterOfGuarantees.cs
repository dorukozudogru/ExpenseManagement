using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class LetterOfGuarantees
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Verilen Kurum")]
        public string Institution { get; set; }

        [Required]
        [ForeignKey(nameof(BankBranches))]
        [DisplayName("Banka/Şube Adı")]
        public int BankBranchId { get; set; }
        [DisplayName("Banka/Şube Adı")]
        public virtual BankBranches BankBranch { get; set; }

        [Required]
        [DisplayName("Tutar")]
        public double Amount { get; set; }

        [Required]
        [DisplayName("Masraf (Binde)")]
        [ForeignKey(nameof(LetterOfGuaranteeCosts))]
        public int LetterOfGuaranteeCostId { get; set; }
        public virtual LetterOfGuaranteeCosts LetterOfGuaranteeCosts { get; set; }

        [Required]
        [DisplayName("Masraf")]
        public double Cost { get; set; }

        [Required]
        [DisplayName("Bitiş Tarihi")]
        public DateTime FinishDate { get; set; }

        [DisplayName("Teminat Mektubu")]
        public byte[] LetterOfGuaranteeImage { get; set; }

        [DisplayName("Teminat Mektubu Formatı")]
        public string LetterOfGuaranteeImageFormat { get; set; }
    }
}