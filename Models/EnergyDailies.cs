using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        public double Kw { get; set; }
    }
}
