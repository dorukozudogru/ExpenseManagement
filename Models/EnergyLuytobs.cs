using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models
{
    public class EnergyLuytobs
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
        [DisplayName("Lüytob")]
        public byte[] Luytob { get; set; }

        [DisplayName("Lüytob Formatı")]
        public string LuytobFormat { get; set; }

        [Required]
        [DisplayName("Fatura")]
        public byte[] Invoice { get; set; }

        [DisplayName("Fatura Formatı")]
        public string InvoiceFormat { get; set; }
    }
}
