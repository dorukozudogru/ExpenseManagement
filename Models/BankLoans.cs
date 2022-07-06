using System;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class BankLoans
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(BankBranches))]
        [DisplayName("Banka/Şube Adı")]
        public int BankBranchId { get; set; }
        [DisplayName("Banka/Şube Adı")]
        public virtual BankBranches BankBranch { get; set; }

        [Required]
        [DisplayName("Alınan Tutar")]
        public double Amount { get; set; }

        [Required]
        [DisplayName("Aylık Ödenecek Tutar")]
        public double InstallmentAmount { get; set; }

        [Required]
        [DisplayName("Toplam Geri Ödenecek Tutar")]
        public double TotalAmountToBeRepaid { get; set; }

        [Required]
        [DisplayName("Faiz Oranı")]
        public double Interest { get; set; }

        [Required]
        [DisplayName("Masraf")]
        public double Cost { get; set; }

        [Required]
        [DisplayName("Kredi Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayName("Kredi Bitiş Tarihi")]
        public DateTime FinishDate { get; set; }
    }
}