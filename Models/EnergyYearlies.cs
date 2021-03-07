using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models
{
    public class EnergyYearlies
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(EnergyMonthlies))]
        public int EnergyMonthlyMonthId { get; set; }
        public virtual EnergyMonthlies EnergyMonthlyMonth { get; set; }

        [Required]
        [DisplayName("Üretilen Kw")]
        public string ProducedKw { get; set; }

        [Required]
        [DisplayName("Tüketilen Kw")]
        public string ConsumedKw { get; set; }

        [Required]
        [DisplayName("Dağıtım Bedeli")]
        public string DistributionFee { get; set; }
    }
}
