using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class ToDoLists
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Sector))]
        public int SectorId { get; set; }
        public virtual Sectors Sector { get; set; }

        [DisplayName("Alacak Türü")]
        public string DeptType { get; set; }

        [Required]
        [DisplayName("Alınacak Kişi/Kurum")]
        public string Debtor { get; set; }

        [Required]
        [DisplayName("Tutar")]
        public double Amount { get; set; }

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

        [DisplayName("Durum")]
        public bool State { get; set; }

        [DisplayName("Tahsilat Tarihi")]
        public DateTime CollectAt { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Oluşturan Kişi")]
        public string CreatedBy { get; set; }
    }
}
