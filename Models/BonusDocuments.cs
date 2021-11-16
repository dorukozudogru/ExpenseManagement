using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class BonusDocuments
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Bonuses))]
        public int BonusId { get; set; }

        [DisplayName("Fatura Görüntüsü")]
        public byte[] InvoiceImage { get; set; }

        [DisplayName("Fatura Görüntüsü Formatı")]
        public string InvoiceImageFormat { get; set; }
    }
}
