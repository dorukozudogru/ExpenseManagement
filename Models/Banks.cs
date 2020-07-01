using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManagement.Models
{
    public class Banks
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(AccountType))]
        public int AccountTypeId { get; set; }
        public virtual AccountTypes AccountType { get; set; }

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

        [DisplayName("Banka/Şube")]
        public string BankBranch { get; set; }
    }
}
