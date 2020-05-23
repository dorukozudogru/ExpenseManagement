using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class Sectors
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Sektör/İş Kolu Adı")]
        public string Name { get; set; }
    }
}
