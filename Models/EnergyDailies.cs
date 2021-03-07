using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models
{
    public class EnergyDaily
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Tarih")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("kW")]
        public string Kw { get; set; }
    }
}
