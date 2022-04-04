using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class EnergyLuytobs
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
        [DisplayName("Lüytob ve Fatura")]
        [ForeignKey(nameof(EnergyLuytobFile))]
        public int EnergyLuytobFileId { get; set; }
        public virtual EnergyLuytobFiles EnergyLuytobFile { get; set; }

        [NotMapped]
        [DisplayName("Lüytob")]
        public byte[] Luytob { get; set; }

        [NotMapped]
        [DisplayName("Fatura")]
        public byte[] Invoice { get; set; }
    }
}
