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

        [DisplayName("Araç Satıldı Mı?")]
        public bool IsSold { get; set; }

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

        [DisplayName("Aracın Satıldığı Kişi")]
        public string Buyer { get; set; }

        [DisplayName("Satış Tarihi")]
        public DateTime? SaleDate { get; set; }

        [DisplayName("Satan Danışman")]
        [ForeignKey(nameof(SoldSalesman))]
        public int? SoldSalesmansId { get; set; }
        [DisplayName("Satan Danışman")]
        public virtual Salesmans SoldSalesman { get; set; }

        [DisplayName("Satış Fiyatı")]
        public double? SaleAmount { get; set; }

        [DisplayName("Satan Danışman Primi")]
        public double? SoldSalesmanBonus { get; set; }

        [DisplayName("Kâr")]
        public double? Profit { get; set; }

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
                    return string.Format($"{this.LicencePlate}");
                }
                return _fullInfo;
            }
            set
            {
                _fullInfo = value;
            }
        }
    }
}
