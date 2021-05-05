using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models.ViewModels
{
    public class ReportViewModel
    {
        public class TotalResponse
        {
            public double TotalAmount { get; set; }

            public string Currency { get; set; }
        }

        public class GeneralResponse
        {
            public int Month { get; set; }

            public string MonthName { get; set; }

            public int Count { get; set; }

            public double? TotalAmount { get; set; }

            public byte? TotalAmountCurrency { get; set; }

            public string TotalAmountCurrencyName { get; set; }
        }

        public class ExpenseEndorsementProfitReport
        {
            public int? Month { get; set; }

            public string MonthName
            {
                get
                {
                    if (this.Month != null)
                    {
                        return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)this.Month);
                    }
                    return null;
                }
            }

            public double TotalAmount { get; set; }

            public byte TotalAmountCurrency { get; set; }

            public string TotalAmountCurrencyName { get; set; }

            public double TotalProfit { get; set; }

            public byte TotalProfitCurrency { get; set; }

            public string TotalProfitCurrencyName { get; set; }

            public double Percent { get; set; }
        }
    }
}
