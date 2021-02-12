using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Enums
{
    public enum CurrencyEnum
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
}
