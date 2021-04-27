using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models.ViewModels
{
    public class ExpenseReportViewModel
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
    }
}
