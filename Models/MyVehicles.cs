using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class MyVehicles
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
        [DisplayName("Yıl")]
        public string ModelYear { get; set; }

        [Required]
        [DisplayName("Trafik Sigortası Tutarı")]
        public double TrafficInsuranceAmount { get; set; }

        [Required]
        [DisplayName("Kasko Tutarı")]
        public double KaskoAmount { get; set; }

        [Required]
        [DisplayName("Gelecek Muayene Tarihi")]
        public DateTime InspectionDate { get; set; }

        [Required]
        [DisplayName("Plaka")]
        public string LicencePlate { get; set; }

        [Required]
        [DisplayName("Aracın Güncel Değeri")]
        public double CurrentValue { get; set; }
    }
}