using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class DepositAccounts
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(BankBranches))]
        [DisplayName("Banka/Şube Adı")]
        public int BankBranchId { get; set; }
        [DisplayName("Banka/Şube Adı")]
        public virtual BankBranches BankBranch { get; set; }

        [NotMapped]
        [DisplayName("Banka/Şube Adı")]
        public string BankBranchName { get; set; }

        [Required]
        [DisplayName("Tutar")]
        public double Amount { get; set; }

        [Required]
        [DisplayName("Tutar Dövizi")]
        public byte AmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Tutar Dövizi")]
        public string AmountCurrencyName { get; set; }

        [NotMapped]
        [DisplayName("Toplam Tutar")]
        public double TotalAmount { get; set; }

        [Required]
        [DisplayName("Vadede Kalacak Gün")]
        public int DayOfDeposit { get; set; }

        [Required]
        [DisplayName("Faiz Oranı")]
        public double Interest { get; set; }

        [Required]
        [DisplayName("Getiri")]
        public double Profit { get; set; }

        [Required]
        [DisplayName("Getiri Dövizi")]
        public byte ProfitCurrency { get; set; }

        [NotMapped]
        [DisplayName("Getiri Dövizi")]
        public string ProfitCurrencyName { get; set; }

        [NotMapped]
        [DisplayName("Toplam Getiri")]
        public double TotalProfitAmount { get; set; }

        [DisplayName("Vade Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [DisplayName("Vade Bitiş Tarihi")]
        public DateTime FinishDate { get; set; }

        [DisplayName("Durum")]
        public bool IsActive { get; set; }
    }
}
