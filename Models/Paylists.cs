using System;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class Paylists
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Tarih")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Tutar")]
        public double Amount { get; set; }

        [Required]
        [DisplayName("Ödeme Yapılacak Kişi/Kurum")]
        public string PersonToBePaid { get; set; }

        [DisplayName("Durum")]
        public bool State { get; set; }
    }
}
