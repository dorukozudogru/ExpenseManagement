using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models
{
    public class EnergyLuytobFiles
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Lüytob")]
        public byte[] Luytob { get; set; }

        [DisplayName("Lüytob Formatı")]
        public string LuytobFormat { get; set; }

        [DisplayName("Fatura")]
        public byte[] Invoice { get; set; }

        [DisplayName("Fatura Formatı")]
        public string InvoiceFormat { get; set; }
    }
}
