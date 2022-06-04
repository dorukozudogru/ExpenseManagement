using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class LetterOfGuaranteeCosts
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Masraf (Binde)")]
        public double Cost { get; set; }
    }
}