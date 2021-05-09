using System.ComponentModel.DataAnnotations;

namespace ExpenseManagement.Enums
{
    public enum MonthEnum
    {
        [Display(Name = "")]
        ZERO = 0,
        [Display(Name = "Ocak")]
        JANUARY = 1,
        [Display(Name = "Şubat")]
        FEBRUARY = 2,
        [Display(Name = "Mart")]
        MARCH = 3,
        [Display(Name = "Nisan")]
        APRIL = 4,
        [Display(Name = "Mayıs")]
        MAY = 5,
        [Display(Name = "Haziran")]
        JUNE = 6,
        [Display(Name = "Temmuz")]
        JULY = 7,
        [Display(Name = "Ağustos")]
        AUGUST = 8,
        [Display(Name = "Eylül")]
        SEPTEMBER = 9,
        [Display(Name = "Ekim")]
        OCTOBER = 10,
        [Display(Name = "Kasım")]
        NOVEMBER = 11,
        [Display(Name = "Aralık")]
        DECEMBER = 12
    }
}
