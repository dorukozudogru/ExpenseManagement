using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Models
{
    public class Audit
    {
        [Key]
        [DisplayName("Log Id")]
        public int Id { get; set; }
        [DisplayName("Tablo Adı")]
        public string TableName { get; set; }
        [DisplayName("Kolon Adı")]
        public string EntityName { get; set; }
        [DisplayName("Yapılan Değişiklik Türü")]
        public string Action { get; set; }
        [DisplayName("Değişiklik Tarihi")]
        public DateTime DateTime { get; set; }
        [DisplayName("Değişikliği Yapan")]
        public string Username { get; set; }
        [DisplayName("Değişikliğin Yapıldığı Id")]
        public string KeyValues { get; set; }
        [DisplayName("Eski Değer")]
        public string OldValues { get; set; }
        [DisplayName("Yeni Değer")]
        public string NewValues { get; set; }
    }
}