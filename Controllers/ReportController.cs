using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseManagement.Data;
using ExpenseManagement.Helpers;
using ExpenseManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using static ExpenseManagement.Models.ViewModels.ReportViewModel;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Muhasebe"))]
    public class ReportController : Controller
    {
        private readonly ExpenseContext _context;

        public ReportController(ExpenseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CumulatedEndorsementReport()
        {
            var requestFormData = Request.Form;

            List<Endorsements> endorsements = await _context.Endorsements
                .Include(i => i.Sector)
                .GroupBy(i => new
                {
                    Year = i.Year,
                    Month = i.Month,
                    AmountCurrency = i.AmountCurrency,
                    SectorName = i.Sector.Name
                })
                .Select(i => new Endorsements
                {
                    SectorName = i.Key.SectorName,
                    Year = i.Key.Year,
                    Month = i.Key.Month,
                    AmountCurrency = i.Key.AmountCurrency,
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .OrderBy(i => i.SectorName)
                .ThenBy(i => i.Month)
                .ThenBy(i => i.Year)
                .ToListAsync();

            endorsements = GetAllEnumNamesHelper.GetEnumName(endorsements);

            var listItems = endorsements
                .GroupBy(i => new
                {
                    i.SectorName,
                    i.AmountCurrencyName,
                    i.Year
                })
                .Select(i => new EndorsementResponse
                {
                    SectorName = i.Key.SectorName,
                    Currency = i.Key.AmountCurrencyName,
                    Year = i.Key.Year,
                    TotalAmount = i.Sum(x => x.TotalAmount)
                })
                .ToList();

            List<EndorsementResponse> endorsementResponse = ProcessCollection(listItems, requestFormData);

            var response = new PaginatedResponse<EndorsementResponse>
            {
                Data = endorsementResponse,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = listItems.Count,
                RecordsTotal = listItems.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CumulatedTotalEndorsementReport()
        {
            List<Endorsements> endorsements = await _context.Endorsements
                .GroupBy(i => new
                {
                    Year = i.Year,
                    Month = i.Month,
                    AmountCurrency = i.AmountCurrency
                })
                .Select(i => new Endorsements
                {
                    Year = i.Key.Year,
                    Month = i.Key.Month,
                    AmountCurrency = i.Key.AmountCurrency,
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .OrderBy(i => i.Month)
                .ThenBy(i => i.Year)
                .ToListAsync();

            endorsements = GetAllEnumNamesHelper.GetEnumName(endorsements);

            var listItems = endorsements
                .GroupBy(i => i.AmountCurrencyName)
                .Select(i => new TotalResponse
                {
                    TotalAmount = i.Sum(x => x.TotalAmount),
                    Currency = i.Key
                })
                .ToList();

            string responseStr = null;

            foreach (var item in listItems)
            {
                responseStr += (item.TotalAmount + " " + item.Currency + " + ");
            }

            var response = new List<string>
            {
                responseStr.Substring(0, responseStr.Length-3)
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> BankVaultsReport()
        {
            List<BankVaults> banks = await _context.BankVaults
                .GroupBy(i => new
                {
                    AmountCurrency = i.AmountCurrency
                })
                .Select(i => new BankVaults
                {
                    AmountCurrency = i.Key.AmountCurrency,
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .ToListAsync();

            banks = GetAllEnumNamesHelper.GetEnumName(banks);

            return Json(banks);
        }

        [HttpPost]
        public async Task<IActionResult> MontlyExpensesReport()
        {
            var requestFormData = Request.Form;

            var expenses = await _context.Expenses
                .Where(i => i.Date.Month >= DateTime.Now.AddMonths(-2).Month)
                .GroupBy(i => new
                {
                    i.Date.Month
                })
                .Select(i => new GeneralResponse
                {
                    Month = i.Key.Month,
                    Count = i.Count(),
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = "₺"
                })
                .ToListAsync();

            expenses = GetAllEnumNamesHelper.GetEnumName(expenses);

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = expenses,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = expenses.Count,
                RecordsTotal = expenses.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> MontlyIncomesReport()
        {
            var requestFormData = Request.Form;

            var incomes = await _context.Incomes
                .Where(i => i.Date.Month >= DateTime.Now.AddMonths(-2).Month)
                .GroupBy(i => new
                {
                    i.Date.Month
                })
                .Select(i => new GeneralResponse
                {
                    Month = i.Key.Month,
                    Count = i.Count(),
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = "₺"
                })
                .ToListAsync();

            incomes = GetAllEnumNamesHelper.GetEnumName(incomes);

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = incomes,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = incomes.Count,
                RecordsTotal = incomes.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult DailyCountReport()
        {
            return Ok(new List<int>
            {
                _context.Expenses.Where(i => i.Date.ToShortDateString() == DateTime.Now.ToShortDateString()).Count(),
                _context.Incomes.Where(i => i.Date.ToShortDateString() == DateTime.Now.ToShortDateString()).Count()
            });
        }
    }
}