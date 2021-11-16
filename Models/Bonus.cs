using System;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class Bonuses
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Prim Türü")]
        public byte BonusType { get; set; }

        [NotMapped]
        [DisplayName("Prim Türü")]
        public string BonusTypeName { get; set; }

        public enum BonusTypeEnum
        {
            [Display(Name = "Servis Primi")]
            SERVICE = 0,
            [Display(Name = "Satış Primi")]
            SALE = 1,
        }

        [Required]
        [DisplayName("Yıl")]
        public int Year { get; set; }

        [Required]
        [DisplayName("Ay")]
        public byte? Month { get; set; }

        [NotMapped]
        [DisplayName("Ay")]
        public string MonthName { get; set; }

        [DisplayName("Tutar")]
        public double Amount { get; set; }

        [DisplayName("Fatura Görüntüsü")]
        public byte[] InvoiceImage { get; set; }

        [DisplayName("Fatura Görüntüsü Formatı")]
        public string InvoiceImageFormat { get; set; }

        [DisplayName("Oluşturan Kişi")]
        public string CreatedBy { get; set; }
    }
}
