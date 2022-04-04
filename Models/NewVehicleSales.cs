using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class NewVehicleSales
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Alınan Araç Şase No")]
        [ForeignKey(nameof(VehiclePurchase))]
        public int VehiclePurchaseId { get; set; }
        public virtual VehiclePurchases VehiclePurchase { get; set; }

        [Required]
        [DisplayName("Satan Danışman")]
        [ForeignKey(nameof(Salesman))]
        public int SalesmanId { get; set; }
        [DisplayName("Satan Danışman")]
        public virtual Salesmans Salesman { get; set; }

        [DisplayName("Plaka")]
        public string LicencePlate { get; set; }

        [Required]
        [DisplayName("Araç Satış Fiyatı")]
        public double SaleAmount { get; set; }

        [Required]
        [DisplayName("Araç Satış Fiyatı Dövizi")]
        public byte SaleAmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Araç Satış Fiyatı Dövizi")]
        public string SaleAmountCurrencyName { get; set; }

        [Required]
        [DisplayName("Alış Tarihi")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DisplayName("Satış Tarihi")]
        public DateTime SaleDate { get; set; }

        [Required]
        [DisplayName("Araç Maliyeti")]
        public double VehicleCost { get; set; }

        [Required]
        [DisplayName("Araç Maliyeti Dövizi")]
        public byte VehicleCostCurrency { get; set; }

        [NotMapped]
        [DisplayName("Araç Maliyeti Dövizi")]
        public string VehicleCostCurrencyName { get; set; }

        [DisplayName("Bandrol")]
        public string WarrantyPlus { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        [DisplayName("ÖTV Muaf Mı?")]
        public bool TaxExempt { get; set; }
    }
}