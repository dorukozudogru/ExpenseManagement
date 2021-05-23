using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models
{
    public class FuelSales
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Sector))]
        [DisplayName("Sektör/İş Kolu Adı")]
        public int SectorId { get; set; }
        [DisplayName("Sektör/İş Kolu Adı")]
        public virtual Sectors Sector { get; set; }

        [Required]
        [ForeignKey(nameof(FuelType))]
        public int FuelTypeId { get; set; }
        public virtual FuelTypes FuelType { get; set; }

        [DisplayName("Tarih")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Net Litre Alım")]
        public double NetLiterPurchase { get; set; }

        [Required]
        [DisplayName("Yakıt Alım (KDV Dahil)")]
        public double FuelPurchase { get; set; }

        [Required]
        [DisplayName("Yakıt Satış Litresi")]
        public double NetLiterSale { get; set; }

        [Required]
        [DisplayName("Yakıt Satış Fiyatı")]
        public double FuelSale { get; set; }
    }
}
