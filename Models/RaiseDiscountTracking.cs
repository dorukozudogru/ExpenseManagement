using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class RaiseDiscountTracking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Sector))]
        [DisplayName("Sektör/İş Kolu Adı")]
        public int SectorId { get; set; }
        [DisplayName("Sektör/İş Kolu Adı")]
        public virtual Sectors Sector { get; set; }

        [DisplayName("Tarih")]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey(nameof(FuelType))]
        public int FuelTypeId { get; set; }
        public virtual FuelTypes FuelType { get; set; }

        [Required]
        [DisplayName("Birim Satış Fiyatı")]
        public double UnitSalePrice { get; set; }

        [Required]
        [DisplayName("Kalan Yakıt Miktarı")]
        public double FuelRemainingAmount { get; set; }

        [Required]
        [DisplayName("Fark")]
        public double Difference { get; set; }
    }
}
