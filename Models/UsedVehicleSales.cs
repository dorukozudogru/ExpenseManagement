using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class UsedVehicleSales
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Alınan Araç Şase No")]
        [ForeignKey(nameof(VehiclePurchase))]
        public int VehiclePurchaseId { get; set; }
        public virtual VehiclePurchases VehiclePurchase { get; set; }

        [Required]
        [DisplayName("Plaka")]
        public string LicencePlate { get; set; }

        [Required]
        [DisplayName("KM")]
        public string KM { get; set; }

        [Required]
        [DisplayName("Alış Tarihi")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DisplayName("Satış Tarihi")]
        public DateTime SaleDate { get; set; }

        [Required]
        [DisplayName("Alan Danışman")]
        [ForeignKey(nameof(PurchasedSalesman))]
        public int PurchasedSalesmanId { get; set; }
        public virtual Salesmans PurchasedSalesman { get; set; }

        [DisplayName("Satan Danışman")]
        [ForeignKey(nameof(SoldSalesman))]
        public int? SoldSalesmanId { get; set; }
        public virtual Salesmans SoldSalesman { get; set; }

        [DisplayName("Araç Alış Fiyatı")]
        public double PurchaseAmount { get; set; }

        [DisplayName("Araç Alış Fiyatı Dövizi")]
        public byte PurchaseAmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Araç Alış Fiyatı Dövizi")]
        public string PurchaseAmountCurrencyName { get; set; }

        [DisplayName("Araç Satış Fiyatı")]
        public double SaleAmount { get; set; }

        [DisplayName("Araç Satış Fiyatı Dövizi")]
        public byte SaleAmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Araç Satış Fiyatı Dövizi")]
        public string SaleAmountCurrencyName { get; set; }

        [DisplayName("Alan Danışman Primi")]
        public double PurchasedSalesmanBonus { get; set; }

        [DisplayName("Alan Danışman Primi Dövizi")]
        public byte PurchasedSalesmanBonusCurrency { get; set; }

        [NotMapped]
        [DisplayName("Alan Danışman Primi Dövizi")]
        public string PurchasedSalesmanBonusCurrencyName { get; set; }

        [DisplayName("Satan Danışman Primi")]
        public double SoldSalesmanBonus { get; set; }

        [DisplayName("Satan Danışman Primi Dövizi")]
        public byte SoldSalesmanBonusCurrency { get; set; }

        [NotMapped]
        [DisplayName("Satan Danışman Primi Dövizi")]
        public string SoldSalesmanBonusCurrencyName { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }
    }
}
