using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class ImportantDocuments
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Doküman Tanımı")]
        public string Definition { get; set; }

        [DisplayName("Doküman")]
        public byte[] Document { get; set; }

        [DisplayName("Doküman Formatı")]
        public string DocumentFormat { get; set; }
    }
}