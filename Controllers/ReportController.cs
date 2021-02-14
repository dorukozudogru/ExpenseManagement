using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ExpenseContext _context;

        public ReportController(ExpenseContext context)
        {
            _context = context;
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public IActionResult Bank()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> BankVaultsReport()
        {
            List<BankVaults> banks = await _context.BankVaults
                .Include(i => i.AccountType)
                .Include(i => i.BankBranch)
                .GroupBy(i => new
                {
                    i.AmountCurrency,
                    BankBranchName = i.BankBranch.Name,
                    AccountTypeName = i.AccountType.Name
                })
                .Select(i => new BankVaults
                {
                    AmountCurrency = i.Key.AmountCurrency,
                    BankBranchName = i.Key.BankBranchName,
                    AccountTypeName = i.Key.AccountTypeName,
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .ToListAsync();

            banks = GetAllEnumNamesHelper.GetEnumName(banks);

            return Json(banks);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> DepositAccountReport()
        {
            List<DepositAccounts> banks = await _context.DepositAccounts
                .Include(i => i.BankBranch)
                .Where(i => i.FinishDate >= DateTime.Now.Date)
                .GroupBy(i => new
                {
                    i.AmountCurrency,
                    BankBranchName = i.BankBranch.Name,
                })
                .Select(i => new DepositAccounts
                {
                    AmountCurrency = i.Key.AmountCurrency,
                    BankBranchName = i.Key.BankBranchName,
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .ToListAsync();

            banks = GetAllEnumNamesHelper.GetEnumName(banks);

            return Json(banks);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> TotalAccountReport()
        {
            List<DepositAccounts> depositAccounts = await _context.DepositAccounts
                .Include(i => i.BankBranch)
                .Where(i => i.FinishDate >= DateTime.Now.Date)
                .GroupBy(i => new
                {
                    i.AmountCurrency,
                })
                .Select(i => new DepositAccounts
                {
                    AmountCurrency = i.Key.AmountCurrency,
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .ToListAsync();

            List<BankVaults> bankVaults = await _context.BankVaults
                .Include(i => i.AccountType)
                .Include(i => i.BankBranch)
                .GroupBy(i => new
                {
                    i.AmountCurrency,
                })
                .Select(i => new BankVaults
                {
                    AmountCurrency = i.Key.AmountCurrency,
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .ToListAsync();

            depositAccounts = GetAllEnumNamesHelper.GetEnumName(depositAccounts);
            bankVaults = GetAllEnumNamesHelper.GetEnumName(bankVaults);

            List<TotalResponse> totalResponse = new List<TotalResponse>();

            foreach (var item in depositAccounts)
            {
                totalResponse.Add(new TotalResponse
                {
                    Currency = item.AmountCurrencyName,
                    TotalAmount = item.TotalAmount
                });
            }

            foreach (var item in bankVaults)
            {
                totalResponse.Add(new TotalResponse
                {
                    Currency = item.AmountCurrencyName,
                    TotalAmount = item.TotalAmount
                });
            }

            var finalTotalAmount = totalResponse
                .GroupBy(t => new
                {
                    t.Currency,
                })
                .Select(t => new TotalResponse
                {
                    Currency = t.Key.Currency,
                    TotalAmount = t.Sum(x => x.TotalAmount)
                })
                .ToList();

            return Json(finalTotalAmount);
        }

        [HttpPost]
        public async Task<IActionResult> MontlyExpensesReport()
        {
            var requestFormData = Request.Form;

            var expenses = await _context.Expenses
                .Where(i => i.Date.Value.Month >= DateTime.Now.AddMonths(-2).Month && i.ExpenseType != 2)
                .GroupBy(i => new
                {
                    i.Date.Value.Month,
                    i.AmountCurrency
                })
                .Select(i => new GeneralResponse
                {
                    Month = i.Key.Month,
                    Count = i.Count(),
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = i.Key.AmountCurrency
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
                .Where(i => i.Month >= DateTime.Now.AddMonths(-2).Month)
                .GroupBy(i => new
                {
                    i.Month,
                    i.AmountCurrency
                })
                .Select(i => new GeneralResponse
                {
                    Month = i.Key.Month,
                    Count = i.Count(),
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = i.Key.AmountCurrency
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
        public async Task<IActionResult> MonthlyCountReport()
        {
            var expenses = await _context.Expenses
                .Where(i => i.Date.Value.Month == DateTime.Now.Month && i.ExpenseType != 2)
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                expenses = expenses
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            var incomes = await _context.Incomes
                .Where(i => i.Month == DateTime.Now.Month)
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                incomes = incomes
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            return Ok(new List<int>
            {
                expenses.Count(),
                incomes.Count()
            });
        }

        public string GetLoggedUserId()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetLoggedUserRole()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }
    }
}