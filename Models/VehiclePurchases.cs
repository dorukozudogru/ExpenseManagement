using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class VehiclePurchases
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(CarModel))]
        public int CarModelId { get; set; }
        public virtual CarModels CarModel { get; set; }
        [NotMapped]
        [DisplayName("Marka")]
        public string CarBrandName { get; set; }

        [DisplayName("Araç Sıfır Mı?")]
        public bool IsNew { get; set; }

        [DisplayName("Araç Satıldı Mı?")]
        public bool IsSold { get; set; }

        [Required]
        [DisplayName("Alış Tarihi")]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Satış Tarihi")]
        public DateTime? SaleDate { get; set; }

        [Required]
        [DisplayName("Valör")]
        public int ValorDate { get; set; }

        [NotMapped]
        [DisplayName("Valör Bitiş Tarihi")]
        public DateTime ValorEndDate { get; set; }

        [DisplayName("Ödeme Tarihi")]
        public DateTime? PaymentDate { get; set; }

        [Required]
        [DisplayName("Şase No")]
        public string Chassis { get; set; }

        [NotMapped]
        private string _fullInfo { get; set; }
        [NotMapped]
        [DisplayName("Araç Bilgileri")]
        public string FullInfo
        {
            get
            {
                if (string.IsNullOrEmpty(_fullInfo))
                {
                    return string.Format($"{this.Chassis}");
                }
                return _fullInfo;
            }
            set
            {
                _fullInfo = value;
            }
        }

        [Required]
        [DisplayName("Alış Fiyatı")]
        public double PurchaseAmount { get; set; }

        [Required]
        [DisplayName("Alış Fiyatı Dövizi")]
        public byte PurchaseAmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Alış Fiyatı Dövizi")]
        public string PurchaseAmountCurrencyName { get; set; }

        [DisplayName("ÖTV Oranı (%)")]
        public int? OTVPercent { get; set; }

        [DisplayName("ÖTV Dahil Fiyatı")]
        public double? OTV { get; set; }

        [DisplayName("KDV Dahil Fiyatı")]
        public double? KDV { get; set; }
        
        [ForeignKey(nameof(RegistrationFees))]
        public int? RegistrationFeeId { get; set; }
        public virtual RegistrationFees RegistrationFee { get; set; }
        [NotMapped]
        public double RegistrationFeeAmount { get; set; }

        [DisplayName("Trafik Tescil Bedeli Dahil Fiyatı")]
        public double IncludingRegistrationFee { get; set; }

        [DisplayName("Satış Fiyatı")]
        public double? SaleAmount { get; set; }

        [Required]
        [DisplayName("Satış Fiyatı Dövizi")]
        public byte? SaleAmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Satış Fiyatı Dövizi")]
        public string SaleAmountCurrencyName { get; set; }

        [Required]
        [DisplayName("Y.A. Ödenecek Tutar")]
        public double AmountToBePaid { get; set; }
    }
}
