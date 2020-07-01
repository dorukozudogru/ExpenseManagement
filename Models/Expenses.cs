using System;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class Expenses
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Sector))]
        public int SectorId { get; set; }
        public virtual Sectors Sector { get; set; }

        [Required]
        [DisplayName("Fatura Tanımı")]
        public string Definition { get; set; }

        [Required]
        [DisplayName("Fatura Tarihi")]
        public DateTime Date { get; set; }

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

        [Required]
        [DisplayName("KDV")]
        public double TAX { get; set; }

        [Required]
        [DisplayName("KDV Dövizi")]
        public double TAXCurrency { get; set; }

        [NotMapped]
        [DisplayName("KDV Dövizi")]
        public string TAXCurrencyName { get; set; }

        public enum TAXCurrencyEnum
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

        [DisplayName("Fatura Görüntüsü")]
        public byte[] InvoiceImage { get; set; }

        [DisplayName("Fatura Görüntüsü Formatı")]
        public string InvoiceImageFormat { get; set; }

        [DisplayName("Durum")]
        public int State { get; set; }

        public enum StateEnum
        {
            [Display(Name = "Aktif")]
            Active = 10,
            [Display(Name = "Pasif")]
            Passive = 20,
        }

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Oluşturan Kişi")]
        public string CreatedBy { get; set; }
    }
}
