using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class Tankers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Yıl")]
        public int Year { get; set; }

        [Required]
        [DisplayName("Ay")]
        public byte? Month { get; set; }

        [NotMapped]
        [DisplayName("Ay")]
        public string MonthName { get; set; }

        [Required]
        [DisplayName("Tanker Sayısı")]
        public int TankerAmount { get; set; }
    }
}
