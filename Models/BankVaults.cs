using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class BankVaults
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(AccountType))]
        public int AccountTypeId { get; set; }
        public virtual AccountTypes AccountType { get; set; }

        [NotMapped]
        [DisplayName("Hesap Türü")]
        public string AccountTypeName { get; set; }

        [Required]
        [DisplayName("Tutar")]
        public double Amount { get; set; }

        [NotMapped]
        [DisplayName("Toplam Tutar")]
        public double TotalAmount { get; set; }

        [Required]
        [DisplayName("Tutar Dövizi")]
        public byte AmountCurrency { get; set; }

        [NotMapped]
        [DisplayName("Tutar Dövizi")]
        public string AmountCurrencyName { get; set; }

        public enum AmountCurrencyEnum
        {
            [Display(Name = "₺")]
            TRY = 0,
            [Display(Name = "$")]
            USD = 1,
            [Display(Name = "€")]
            EUR = 2,
            [Display(Name = "£")]
            GBP = 3
        }

        [Required]
        [ForeignKey(nameof(BankBranches))]
        [DisplayName("Banka/Şube Adı")]
        public int BankBranchId { get; set; }
        [DisplayName("Banka/Şube Adı")]
        public virtual BankBranches BankBranch { get; set; }

        [NotMapped]
        [DisplayName("Banka/Şube Adı")]
        public string BankBranchName { get; set; }
    }
}
