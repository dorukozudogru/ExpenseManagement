using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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

        [DisplayName("Araç Satıldı Mı?")]
        public bool IsSold { get; set; }

        [Required]
        [DisplayName("Alış Tarihi")]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Satış Tarihi")]
        public DateTime? SaleDate { get; set; }

        [Required]
        [DisplayName("Şase No")]
        public string Chassis { get; set; }

        [Required]
        [DisplayName("Alış Fiyatı")]
        public double PurchaseAmount { get; set; }

        [DisplayName("Satış Fiyatı")]
        public double? SaleAmount { get; set; }
    }
}
