using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class UsedVehiclePurchases
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(CarModel))]
        public int CarModelId { get; set; }
        public virtual CarModels CarModel { get; set; }
        [NotMapped]
        [DisplayName("Marka")]
        public string CarBrandName { get; set; }

        [Required]
        [DisplayName("KM")]
        public string KM { get; set; }

        [Required]
        [DisplayName("Plaka")]
        public string LicencePlate { get; set; }

        [Required]
        [DisplayName("Aracın Alındığı Kişi")]
        public string Seller { get; set; }

        [Required]
        [DisplayName("Alış Tarihi")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DisplayName("Alan Danışman")]
        [ForeignKey(nameof(PurchasedSalesman))]
        public int PurchasedSalesmanId { get; set; }
        [DisplayName("Alan Danışman")]
        public virtual Salesmans PurchasedSalesman { get; set; }

        [Required]
        [DisplayName("Alış Fiyatı")]
        public double PurchaseAmount { get; set; }

        [Required]
        [DisplayName("Alan Danışman Primi")]
        public double PurchasedSalesmanBonus { get; set; }

        [Required]
        [DisplayName("Aracın Satıldığı Kişi")]
        public string Buyer { get; set; }

        [Required]
        [DisplayName("Satış Tarihi")]
        public DateTime SaleDate { get; set; }

        [Required]
        [DisplayName("Satan Danışman")]
        [ForeignKey(nameof(SoldSalesman))]
        public int? SoldSalesmanId { get; set; }
        [DisplayName("Satan Danışman")]
        public virtual Salesmans SoldSalesman { get; set; }

        [Required]
        [DisplayName("Satış Fiyatı")]
        public double SaleAmount { get; set; }

        [Required]
        [DisplayName("Satan Danışman Primi")]
        public double SoldSalesmanBonus { get; set; }

        [Required]
        [DisplayName("Kâr")]
        public double Profit { get; set; }
    }
}
