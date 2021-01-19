using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models
{
    public class Salesmans
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Danışman Adı-Soyadı")]
        public string Name { get; set; }
    }
}
