using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models.ViewModels
{
    public class ReportViewModel
    {
        public class EndorsementResponse
        {
            public string SectorName { get; set; }

            public int Year { get; set; }

            public string Month { get; set; }

            public string Currency { get; set; }

            public double TotalAmount { get; set; }
        }

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
    }
}
