using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        [DisplayName("Ay")]
        public string MonthName { get; set; }
    }
}
