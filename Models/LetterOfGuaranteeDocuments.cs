using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class LetterOfGuaranteeDocuments
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(LetterOfGuarantees))]
        public int LetterOfGuaranteeId { get; set; }

        [DisplayName("Teminat Mektubu")]
        public byte[] LetterOfGuaranteeImage { get; set; }

        [DisplayName("Teminat Mektubu Formatı")]
        public string LetterOfGuaranteeImageFormat { get; set; }
    }
}
