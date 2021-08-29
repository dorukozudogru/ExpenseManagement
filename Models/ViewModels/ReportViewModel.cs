using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

            public int? Year { get; set; }

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

            public int Year { get; set; }

            public double TotalAmount { get; set; }

            public byte TotalAmountCurrency { get; set; }

            public string TotalAmountCurrencyName { get; set; }

            public double TotalProfit { get; set; }

            public byte TotalProfitCurrency { get; set; }

            public string TotalProfitCurrencyName { get; set; }

            public double DifferenceAmount { get; set; }

            public double FuelPurchaseAmount { get; set; }

            public double TotalPurchaseSaleProfit { get; set; }

            public double Percent { get; set; }

            public string BankBranchName { get; set; }
        }

        public class RDTResponse
        {
            public string SectorName { get; set; }

            public string FuelTypeName { get; set; }

            public DateTime Date { get; set; }

            public double DifferenceAmount { get; set; }

            [ForeignKey(nameof(FuelSale))]
            public int FuelSaleId { get; set; }
            public virtual FuelSales FuelSale { get; set; }
        }

        public class ToDoListResponse
        {
            public string SectorName { get; set; }

            public double TotalAmount { get; set; }

            public byte TotalAmountCurrency { get; set; }

            public string TotalAmountCurrencyName { get; set; }
        }

        public class AuditResponse
        {
            public string ActionName { get; set; }

            public string TableName { get; set; }

            public int ActionCount { get; set; }
        }
    }
}
