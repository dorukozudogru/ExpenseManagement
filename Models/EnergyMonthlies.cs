using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models
{
    public class EnergyMonthlies
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Ay")]
        public byte? Month { get; set; }

        [NotMapped]
        [DisplayName("Ay")]
        public string MonthName { get; set; }

        [Required]
        [DisplayName("Fatura Tutarı (KDV Hariç)")]
        public double? Amount { get; set; }

        [DisplayName("Fatura Tutarı (KDV Hariç) Dövizi")]
        public byte? AmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Fatura Tutarı (KDV Hariç) Dövizi")]
        public string AmountCurrencyName { get; set; }

        [Required]
        [DisplayName("kW")]
        public string Kw { get; set; }
    }
}
