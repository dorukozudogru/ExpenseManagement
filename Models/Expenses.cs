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
        [DisplayName("Gider Türü")]
        public byte ExpenseType { get; set; }

        [NotMapped]
        [DisplayName("Gider Türü")]
        public string ExpenseTypeName { get; set; }

        public enum ExpenseTypeEnum
        {
            [Display(Name = "Satınalma")]
            PURCHASE = 0,
            [Display(Name = "Gider")]
            EXPENSE = 1,
            [Display(Name = "Maaş")]
            SALARY = 2,
        }

        [Required]
        [ForeignKey(nameof(Sector))]
        public int SectorId { get; set; }
        public virtual Sectors Sector { get; set; }
        [NotMapped]
        public string SectorName { get; set; }

        //[ForeignKey(nameof(Supplier))]
        //public int? SupplierId { get; set; }
        //public virtual Suppliers Supplier { get; set; }
        //[NotMapped]
        //public string SupplierName { get; set; }

        [DisplayName("Satıcı/Tedarikçi")]
        public string SupplierDef { get; set; }

        [Required]
        [DisplayName("Gider Tanımı")]
        public string Definition { get; set; }

        [DisplayName("Gider Tarihi")]
        public DateTime? Date { get; set; }

        [DisplayName("Son Ödeme Tarihi")]
        public DateTime? LastPaymentDate { get; set; }

        [DisplayName("Tutar")]
        public double? Amount { get; set; }

        [DisplayName("Tutar Dövizi")]
        public byte? AmountCurrency { get; set; }

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

        [DisplayName("KDV")]
        public double? TAX { get; set; }

        [DisplayName("KDV Dövizi")]
        public double? TAXCurrency { get; set; }

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

        #region ForSalary
        [DisplayName("Ay (Maaş)")]
        public byte? Month { get; set; }

        [NotMapped]
        [DisplayName("Ay (Maaş)")]
        public string MonthName { get; set; }

        [DisplayName("Net Tutar (Maaş)")]
        public double? SalaryAmount { get; set; }

        [DisplayName("Net Tutar Dövizi (Maaş)")]
        public byte? SalaryAmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Net Tutar Dövizi (Maaş)")]
        public string SalaryAmountCurrencyName { get; set; }

        public enum SalaryAmountCurrencyEnum
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
        #endregion

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Oluşturan Kişi")]
        public string CreatedBy { get; set; }
    }
}
