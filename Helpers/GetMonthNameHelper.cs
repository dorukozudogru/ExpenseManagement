using ExpenseManagement.Enums;

namespace ExpenseManagement.Helpers
{
    public class GetMonthNameHelper
    {
        public static string GetMonth(byte? monthId)
        {
            string monthName = "";

            switch (monthId)
            {
                case 1:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JANUARY);
                    break;
                case 2:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.FEBRUARY);
                    break;
                case 3:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MARCH);
                    break;
                case 4:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.APRIL);
                    break;
                case 5:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MAY);
                    break;
                case 6:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JUNE);
                    break;
                case 7:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JULY);
                    break;
                case 8:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.AUGUST);
                    break;
                case 9:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.SEPTEMBER);
                    break;
                case 10:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.OCTOBER);
                    break;
                case 11:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.NOVEMBER);
                    break;
                case 12:
                    monthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.DECEMBER);
                    break;
                default:
                    break;
            }
            return monthName;
        }
    }
}
