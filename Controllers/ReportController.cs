using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ExpenseManagement.Data;
using ExpenseManagement.Helpers;
using ExpenseManagement.Models;
using ExpenseManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using static ExpenseManagement.Helpers.AddExportAuditHelper;
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

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public IActionResult ExpenseReport()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe, Plaza"))]
        public IActionResult EndorsementReport()
        {
            return View();
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public IActionResult EnergyProfitReport()
        {
            return View();
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe, Petrol"))]
        public IActionResult POSReport()
        {
            var sectors = _context.Sectors.Where(s => s.Name.Contains("SHELL")).ToList();
            sectors.Add(new Sectors
            {
                Id = 0,
                Name = ""
            });
            ViewData["SectorId"] = new SelectList(sectors.OrderBy(s => s.Id), "Id", "Name");
            return View();
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe, Petrol"))]
        public IActionResult RaiseDiscountTrackingReport()
        {
            var sectors = _context.Sectors.Where(s => s.Name.Contains("SHELL")).ToList();
            sectors.Add(new Sectors
            {
                Id = 0,
                Name = ""
            });

            var fuelTypes = _context.FuelTypes.ToList();
            fuelTypes.Add(new FuelTypes
            {
                Id = 0,
                Name = ""
            });

            ViewData["SectorId"] = new SelectList(sectors.OrderBy(s => s.Id), "Id", "Name");
            ViewData["FuelTypeId"] = new SelectList(fuelTypes.OrderBy(s => s.Id), "Id", "Name");
            return View();
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe, Petrol"))]
        public IActionResult FuelSaleReport()
        {
            var sectors = _context.Sectors.Where(s => s.Name.Contains("SHELL")).ToList();
            sectors.Add(new Sectors
            {
                Id = 0,
                Name = ""
            });

            var fuelTypes = _context.FuelTypes.ToList();
            fuelTypes.Add(new FuelTypes
            {
                Id = 0,
                Name = ""
            });

            ViewData["SectorId"] = new SelectList(sectors.OrderBy(s => s.Id), "Id", "Name");
            ViewData["FuelTypeId"] = new SelectList(fuelTypes.OrderBy(s => s.Id), "Id", "Name");
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
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                expenses = expenses
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            var expensesFinal = expenses
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
                .ToList();

            expensesFinal = GetAllEnumNamesHelper.GetEnumName(expensesFinal);

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = expensesFinal,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = expensesFinal.Count,
                RecordsTotal = expensesFinal.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> MontlyIncomesReport()
        {
            var requestFormData = Request.Form;

            var incomes = await _context.Incomes
                .Where(i => i.Date.Month >= DateTime.Now.AddMonths(-2).Month)
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                incomes = incomes
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            var incomesFinal = incomes
                .GroupBy(i => new
                {
                    i.Date.Month,
                    i.AmountCurrency
                })
                .Select(i => new GeneralResponse
                {
                    Month = i.Key.Month,
                    Count = i.Count(),
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = i.Key.AmountCurrency
                })
                .ToList();

            incomesFinal = GetAllEnumNamesHelper.GetEnumName(incomesFinal);

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = incomesFinal,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = incomesFinal.Count,
                RecordsTotal = incomesFinal.Count
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
                .Where(i => i.Date.Month == DateTime.Now.Month)
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

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> ExpenseReportPost(DateTime startDate, DateTime finishDate, byte expenseType)
        {
            var expenseContext = await _context.Expenses
                .Where(e => e.ExpenseType == expenseType && e.Date >= startDate && e.Date <= finishDate && e.Amount != null)
                .GroupBy(e => new
                {
                    e.Date.Value.Month,
                    e.AmountCurrency
                })
                .Select(e => new ExpenseEndorsementProfitReport
                {
                    Month = e.Key.Month,
                    TotalAmount = e.Sum(i => (double)i.Amount),
                    TotalAmountCurrency = (byte)e.Key.AmountCurrency
                })
                .AsNoTracking()
                .ToListAsync();

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(expenseContext);

            var response = new PaginatedResponse<ExpenseEndorsementProfitReport>
            {
                Data = expenseContext,
                Draw = 1,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> ExpenseReportTotalPost(DateTime startDate, DateTime finishDate, byte expenseType)
        {
            var expenseContext = await _context.Expenses
                .Where(e => e.ExpenseType == expenseType && e.Date >= startDate && e.Date <= finishDate && e.Amount != null)
                .GroupBy(e => new
                {
                    e.AmountCurrency
                })
                .Select(e => new ExpenseEndorsementProfitReport
                {
                    TotalAmount = e.Sum(i => (double)i.Amount),
                    TotalAmountCurrency = (byte)e.Key.AmountCurrency
                })
                .AsNoTracking()
                .ToListAsync();

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            return Ok(expenseContext);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe, Plaza"))]
        public async Task<IActionResult> EndorsementReportPost(int year)
        {
            var usedVehicleSale = await _context.UsedVehicleSales
                .Where(e => e.SaleDate.Year == year)
                .GroupBy(e => new
                {
                    e.SaleDate.Month,
                    e.SaleAmountCurrency
                })
                .Select(e => new ExpenseEndorsementProfitReport
                {
                    Month = e.Key.Month,
                    TotalAmount = e.Sum(i => i.SaleAmount),
                    TotalAmountCurrency = e.Key.SaleAmountCurrency,
                    TotalProfit = e.Sum(i => i.SaleAmount) - e.Sum(i => i.PurchaseAmount),
                    TotalProfitCurrency = e.Key.SaleAmountCurrency
                })
                .AsNoTracking()
                .ToListAsync();

            var newVehicleSale = await _context.NewVehicleSales
                .Where(e => e.SaleDate.Year == year)
                .GroupBy(e => new
                {
                    e.SaleDate.Month,
                    e.SaleAmountCurrency
                })
                .Select(e => new ExpenseEndorsementProfitReport
                {
                    Month = e.Key.Month,
                    TotalAmount = e.Sum(i => i.SaleAmount),
                    TotalAmountCurrency = e.Key.SaleAmountCurrency,
                    TotalProfit = e.Sum(i => i.SaleAmount) - e.Sum(i => i.VehicleCost),
                    TotalProfitCurrency = e.Key.SaleAmountCurrency
                })
                .AsNoTracking()
                .ToListAsync();

            foreach (var item in usedVehicleSale)
            {
                newVehicleSale.Add(item);
            }

            var totalSale = newVehicleSale
                .GroupBy(e => new
                {
                    e.Month,
                    e.TotalAmountCurrency,
                    e.TotalProfitCurrency
                })
                .Select(e => new ExpenseEndorsementProfitReport
                {
                    Month = e.Key.Month,
                    TotalAmount = e.Sum(i => i.TotalAmount),
                    TotalAmountCurrency = e.Key.TotalAmountCurrency,
                    TotalProfit = e.Sum(i => i.TotalProfit),
                    TotalProfitCurrency = e.Key.TotalProfitCurrency
                })
                .OrderBy(i => i.Month)
                .ToList();

            totalSale = GetAllEnumNamesHelper.GetEnumName(totalSale);

            foreach (var item in totalSale)
            {
                item.Percent = (item.TotalProfit * 100) / item.TotalAmount;
            }

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(totalSale);

            var response = new PaginatedResponse<ExpenseEndorsementProfitReport>
            {
                Data = totalSale,
                Draw = 1,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe, Plaza"))]
        public async Task<IActionResult> EndorsementReportTotalPost(int year)
        {
            var usedVehicleSale = await _context.UsedVehicleSales
                .Where(e => e.SaleDate.Year == year)
                .GroupBy(e => new
                {
                    e.SaleAmountCurrency
                })
                .Select(e => new ExpenseEndorsementProfitReport
                {
                    TotalAmount = e.Sum(i => i.SaleAmount),
                    TotalAmountCurrency = e.Key.SaleAmountCurrency,
                    TotalProfit = e.Sum(i => i.SaleAmount) - e.Sum(i => i.PurchaseAmount),
                    TotalProfitCurrency = e.Key.SaleAmountCurrency
                })
                .AsNoTracking()
                .ToListAsync();

            var newVehicleSale = await _context.NewVehicleSales
                .Where(e => e.SaleDate.Year == year)
                .GroupBy(e => new
                {
                    e.SaleAmountCurrency
                })
                .Select(e => new ExpenseEndorsementProfitReport
                {
                    TotalAmount = e.Sum(i => i.SaleAmount),
                    TotalAmountCurrency = e.Key.SaleAmountCurrency,
                    TotalProfit = e.Sum(i => i.SaleAmount) - e.Sum(i => i.VehicleCost),
                    TotalProfitCurrency = e.Key.SaleAmountCurrency
                })
                .AsNoTracking()
                .ToListAsync();

            foreach (var item in usedVehicleSale)
            {
                newVehicleSale.Add(item);
            }

            var totalSale = newVehicleSale
                .GroupBy(e => new
                {
                    e.TotalAmountCurrency,
                    e.TotalProfitCurrency
                })
                .Select(e => new ExpenseEndorsementProfitReport
                {
                    TotalAmount = e.Sum(i => i.TotalAmount),
                    TotalAmountCurrency = e.Key.TotalAmountCurrency,
                    TotalProfit = e.Sum(i => i.TotalProfit),
                    TotalProfitCurrency = e.Key.TotalProfitCurrency
                })
                .OrderBy(i => i.Month)
                .ToList();

            totalSale = GetAllEnumNamesHelper.GetEnumName(totalSale);

            foreach (var item in totalSale)
            {
                item.Percent = (item.TotalProfit * 100) / item.TotalAmount;
            }

            return Ok(totalSale);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> EnergyProfitReportPost(int year)
        {
            var energyMonthlies = await _context.EnergyMonthlies
                .Where(em => em.Year == year)
                .GroupBy(e => new
                {
                    e.Month
                })
                .Select(em => new ExpenseEndorsementProfitReport
                {
                    Month = em.Key.Month,
                    TotalProfit = em.Sum(i => i.Amount)
                })
                .AsNoTracking()
                .ToListAsync();

            energyMonthlies = GetAllEnumNamesHelper.GetEnumName(energyMonthlies);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(energyMonthlies);

            var response = new PaginatedResponse<ExpenseEndorsementProfitReport>
            {
                Data = energyMonthlies,
                Draw = 1,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe, Petrol"))]
        public async Task<IActionResult> POSReportPost(int year, int monthId, int sectorId, DateTime date)
        {
            var posReport = await _context.PointOfSale.ToListAsync();

            if (year != 0)
            {
                posReport = posReport.Where(e => e.Year == year).ToList();
            }
            if (sectorId != 0)
            {
                posReport = posReport.Where(e => e.SectorId == sectorId).ToList();
            }
            if (monthId != 0)
            {
                posReport = posReport.Where(e => e.Month == monthId).ToList();
            }
            if (date != DateTime.MinValue)
            {
                posReport = posReport.Where(e => e.Date == date).ToList();
            }

            var posReportFinal = posReport
                .GroupBy(e => new
                {
                    e.BankBranchId,
                    e.Month,
                    e.Year
                })
                .Select(p => new ExpenseEndorsementProfitReport
                {
                    Month = p.Key.Month,
                    Year = p.Key.Year,
                    TotalProfit = p.Sum(i => i.Amount),
                    BankBranchName = _context.BankBranches.FirstOrDefault(bb => bb.Id == p.Key.BankBranchId).Name.ToString()
                })
                .ToList();

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(posReportFinal);

            var response = new PaginatedResponse<ExpenseEndorsementProfitReport>
            {
                Data = posReportFinal,
                Draw = 1,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe, Petrol"))]
        public async Task<IActionResult> RaiseDiscountTrackingReportPost(int sectorId, int fuelTypeId, DateTime startDate, DateTime finishDate)
        {
            var rdtReport = await _context.RaiseDiscountTracking
                .Include(s => s.Sector)
                .Include(f => f.FuelType)
                .ToListAsync();

            if (sectorId != 0)
            {
                rdtReport = rdtReport.Where(e => e.SectorId == sectorId).ToList();
            }
            if (fuelTypeId != 0)
            {
                rdtReport = rdtReport.Where(e => e.FuelTypeId == fuelTypeId).ToList();
            }
            if (startDate != DateTime.MinValue && finishDate != DateTime.MinValue)
            {
                rdtReport = rdtReport.Where(e => e.Date >= startDate && e.Date <= finishDate).ToList();
            }

            var rdtReportFinal = rdtReport
                .GroupBy(e => new
                {
                    e.Sector,
                    e.FuelType,
                    e.Date
                })
                .Select(p => new RDTResponse
                {
                    SectorName = p.Key.Sector.Name,
                    FuelTypeName = p.Key.FuelType.Name,
                    Date = p.Key.Date,
                    FuelSale = _context.FuelSales.FirstOrDefault(f => f.Date.Day == p.Key.Date.Day && f.Date.Month == p.Key.Date.Month && f.Date.Year == p.Key.Date.Year)
                    //DifferenceAmount = p.Sum(r => r.Difference) * p.Sum(r => r.FuelRemainingAmount)
                })
                .ToList();

            foreach (var item in rdtReportFinal)
            {
                if (item.FuelSale != null)
                {
                    //item.DifferenceAmount = item.DifferenceAmount
                }
            }

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(rdtReportFinal);

            var response = new PaginatedResponse<RDTResponse>
            {
                Data = rdtReportFinal,
                Draw = 1,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe, Petrol"))]
        public async Task<IActionResult> FuelSaleReportPost(int sectorId, int fuelTypeId, DateTime startDate, DateTime finishDate)
        {
            var fsReport = await _context.FuelSales
                .Include(s => s.Sector)
                .Include(f => f.FuelType)
                .ToListAsync();

            if (sectorId != 0)
            {
                fsReport = fsReport.Where(e => e.SectorId == sectorId).ToList();
            }
            if (fuelTypeId != 0)
            {
                fsReport = fsReport.Where(e => e.FuelTypeId == fuelTypeId).ToList();
            }
            if (startDate != DateTime.MinValue && finishDate != DateTime.MinValue)
            {
                fsReport = fsReport.Where(e => e.Date >= startDate && e.Date <= finishDate).ToList();
            }

            var fsReportFinal = fsReport
                .GroupBy(e => new
                {
                    e.Date.Month
                })
                .Select(p => new ExpenseEndorsementProfitReport
                {
                    TotalAmount = p.Sum(f => f.NetLiterSale) * p.Sum(f => f.FuelSale),
                    DifferenceAmount = p.Sum(f => f.FuelSale) - p.Sum(f => f.FuelPurchase),
                    TotalPurchaseSaleProfit = (p.Sum(f => f.FuelSale) - p.Sum(f => f.FuelPurchase)) / p.Sum(f => f.FuelPurchase),
                    FuelPurchaseAmount = p.Sum(f => f.FuelPurchase) * p.Sum(f => f.NetLiterSale),
                    TotalProfit = (p.Sum(f => f.NetLiterSale) * p.Sum(f => f.FuelSale)) - p.Sum(f => f.FuelPurchase)
                })
                .ToList();

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(fsReportFinal);

            var response = new PaginatedResponse<ExpenseEndorsementProfitReport>
            {
                Data = fsReportFinal,
                Draw = 1,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> YearlyExpenseReport(string year)
        {
            var requestFormData = Request.Form;

            var salary = await _context.Expenses
                .Where(i => i.ExpenseType == 2 && i.Year.ToString() == year)
                .GroupBy(i => new
                {
                    i.Year,
                    i.SalaryAmountCurrency
                })
                .Select(i => new GeneralResponse
                {
                    SectorName = "Maaşlar",
                    TotalAmount = i.Sum(x => x.SalaryAmount),
                    TotalAmountCurrency = i.Key.SalaryAmountCurrency
                })
                .ToListAsync();

            var expense = await _context.Expenses
                .Where(i => i.ExpenseType != 2 && i.Date.Value.Year.ToString() == year)
                .GroupBy(i => new
                {
                    i.Year,
                    i.AmountCurrency
                })
                .Select(i => new GeneralResponse
                {
                    SectorName = "Giderler",
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = i.Key.AmountCurrency
                })
                .ToListAsync();

            var final = new List<GeneralResponse>();

            if (expense.Count != 0)
            {
                expense = GetAllEnumNamesHelper.GetEnumName(expense);
                foreach (var item in expense)
                {
                    final.Add(item);
                }
            }
            if (salary.Count != 0)
            {
                salary = GetAllEnumNamesHelper.GetEnumName(salary);
                foreach (var item in salary)
                {
                    final.Add(item);
                }
            }

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = final,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = final.Count,
                RecordsTotal = final.Count
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> YearlyExpenseReportTotalPost(string year)
        {
            var salary = await _context.Expenses
                .Where(i => i.ExpenseType == 2 && i.Year.ToString() == year)
                .GroupBy(i => new
                {
                    i.SalaryAmountCurrency
                })
                .Select(i => new GeneralResponse
                {
                    TotalAmount = i.Sum(x => (double)x.SalaryAmount),
                    TotalAmountCurrency = i.Key.SalaryAmountCurrency
                })
                .ToListAsync();

            var expense = await _context.Expenses
                .Where(i => i.ExpenseType != 2 && i.Date.Value.Year.ToString() == year)
                .GroupBy(i => new
                {
                    i.AmountCurrency
                })
                .Select(i => new GeneralResponse
                {
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = i.Key.AmountCurrency
                })
                .ToListAsync();

            var final = new List<GeneralResponse>();

            if (expense.Count != 0)
            {
                expense = GetAllEnumNamesHelper.GetEnumName(expense);
                foreach (var item in expense)
                {
                    final.Add(item);
                }
            }
            if (salary.Count != 0)
            {
                salary = GetAllEnumNamesHelper.GetEnumName(salary);
                foreach (var item in salary)
                {
                    final.Add(item);
                }
            }

            var totalFinal = new List<GeneralResponse>();

            totalFinal = final
                .GroupBy(f => f.TotalAmountCurrencyName)
                .Select(f => new GeneralResponse
                {
                    TotalAmount = f.Sum(x => x.TotalAmount),
                    TotalAmountCurrencyName = f.Key.First().ToString()
                })
                .ToList();

            return Ok(totalFinal);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> YearlyIncomeReport(string year)
        {
            var requestFormData = Request.Form;

            var bonus = await _context.Bonuses
                .Where(i => i.Year.ToString() == year)
                .GroupBy(i => i.Year)
                .Select(i => new GeneralResponse
                {
                    SectorName = "Primler",
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .ToListAsync();

            var energy = await _context.EnergyMonthlies
                .Where(i => i.Year.ToString() == year)
                .GroupBy(i => i.Year)
                .Select(i => new GeneralResponse
                {
                    SectorName = "Enerji",
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .ToListAsync();

            var petrol = await _context.PetrolNetProfit
                .Where(i => i.Year.ToString() == year)
                .GroupBy(i => i.Year)
                .Select(i => new GeneralResponse
                {
                    SectorName = "Petrol",
                    TotalAmount = i.Sum(x => x.NetProfit)
                })
               .ToListAsync();

            var newVehicle = await _context.VehiclePurchases
                .Where(i => i.IsSold == true && i.SaleDate.Value.Year.ToString() == year)
                .GroupBy(i => new
                {
                    i.SaleDate.Value.Year,
                    i.PurchaseAmountCurrency
                })
                .Select(i => new GeneralResponse
                {
                    SectorName = "Sıfır Araç Satışı",
                    TotalAmount = i.Sum(x => x.SaleAmount) - i.Sum(x => x.IncludingRegistrationFee),
                    TotalAmountCurrency = i.Key.PurchaseAmountCurrency
                })
                .ToListAsync();

            var usedVehicle = await _context.UsedVehiclePurchases
                .Where(i => i.IsSold == true && i.SaleDate.Value.Year.ToString() == year)
                .GroupBy(i => i.SaleDate.Value.Year)
                .Select(i => new GeneralResponse
                {
                    SectorName = "2. El Araç Satışı",
                    TotalAmount = i.Sum(x => x.SaleAmount) - i.Sum(x => x.PurchaseAmount)
                })
                .ToListAsync();

            var interestIncome = await _context.InterestIncomes
                .Where(i => i.Year.ToString() == year)
                .GroupBy(i => i.Year)
                .Select(i => new GeneralResponse
                {
                    SectorName = "Faiz Gelirleri",
                    TotalAmount = i.Sum(x => x.Amount)
                })
                .ToListAsync();

            var final = new List<GeneralResponse>();

            if (newVehicle.Count != 0)
            {
                newVehicle = GetAllEnumNamesHelper.GetEnumName(newVehicle);
                foreach (var item in newVehicle)
                {
                    final.Add(item);
                }
            }
            if (usedVehicle.Count != 0)
            {
                usedVehicle.First().TotalAmountCurrencyName = "₺";
                final.Add(usedVehicle.First());
            }
            if (energy.Count != 0)
            {
                energy.First().TotalAmountCurrencyName = "₺";
                final.Add(energy.First());
            }
            if (petrol.Count != 0)
            {
                petrol.First().TotalAmountCurrencyName = "₺";
                final.Add(petrol.First());
            }
            if (bonus.Count != 0)
            {
                bonus.First().TotalAmountCurrencyName = "₺";
                final.Add(bonus.First());
            }
            if (interestIncome.Count != 0)
            {
                interestIncome.First().TotalAmountCurrencyName = "₺";
                final.Add(interestIncome.First());
            }

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = final,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = final.Count,
                RecordsTotal = final.Count
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public async Task<IActionResult> YearlyIncomeReportTotalPost(string year)
        {
            var bonus = await _context.Bonuses
                .Where(i => i.Year.ToString() == year)
                .GroupBy(i => i.Year)
                .Select(i => new GeneralResponse
                {
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = 0
                })
                .ToListAsync();

            var energy = await _context.EnergyMonthlies
                .Where(i => i.Year.ToString() == year)
                .GroupBy(i => i.Year)
                .Select(i => new GeneralResponse
                {
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = 0
                })
                .ToListAsync();

            var petrol = await _context.PetrolNetProfit
                .Where(i => i.Year.ToString() == year)
                .GroupBy(i => i.Year)
                .Select(i => new GeneralResponse
                {
                    TotalAmount = i.Sum(x => x.NetProfit),
                    TotalAmountCurrency = 0
                })
               .ToListAsync();

            var newVehicle = await _context.VehiclePurchases
                .Where(i => i.IsSold == true && i.SaleDate.Value.Year.ToString() == year)
                .GroupBy(i => new
                {
                    i.PurchaseAmountCurrency
                })
                .Select(i => new GeneralResponse
                {
                    TotalAmount = i.Sum(x => x.SaleAmount) - i.Sum(x => x.IncludingRegistrationFee),
                    TotalAmountCurrency = i.Key.PurchaseAmountCurrency
                })
                .ToListAsync();

            var usedVehicle = await _context.UsedVehiclePurchases
                .Where(i => i.IsSold == true && i.SaleDate.Value.Year.ToString() == year)
                .GroupBy(i => i.SaleDate.Value.Year)
                .Select(i => new GeneralResponse
                {
                    TotalAmount = i.Sum(x => x.SaleAmount) - i.Sum(x => x.PurchaseAmount),
                    TotalAmountCurrency = 0
                })
                .ToListAsync();

            var interestIncome = await _context.InterestIncomes
                .Where(i => i.Year.ToString() == year)
                .GroupBy(i => i.Year)
                .Select(i => new GeneralResponse
                {
                    TotalAmount = i.Sum(x => x.Amount),
                    TotalAmountCurrency = 0
                })
                .ToListAsync();

            var final = new List<GeneralResponse>();

            if (newVehicle.Count != 0)
            {
                newVehicle = GetAllEnumNamesHelper.GetEnumName(newVehicle);
                foreach (var item in newVehicle)
                {
                    final.Add(item);
                }
            }
            if (usedVehicle.Count != 0)
            {
                usedVehicle.First().TotalAmountCurrencyName = "₺";
                final.Add(usedVehicle.First());
            }
            if (energy.Count != 0)
            {
                energy.First().TotalAmountCurrencyName = "₺";
                final.Add(energy.First());
            }
            if (petrol.Count != 0)
            {
                petrol.First().TotalAmountCurrencyName = "₺";
                final.Add(petrol.First());
            }
            if (bonus.Count != 0)
            {
                bonus.First().TotalAmountCurrencyName = "₺";
                final.Add(bonus.First());
            }
            if (interestIncome.Count != 0)
            {
                interestIncome.First().TotalAmountCurrencyName = "₺";
                final.Add(interestIncome.First());
            }

            var totalFinal = new List<GeneralResponse>();

            totalFinal = final
                .GroupBy(f => f.TotalAmountCurrencyName)
                .Select(f => new GeneralResponse
                {
                    TotalAmount = f.Sum(x => x.TotalAmount),
                    TotalAmountCurrencyName = f.Key.First().ToString()
                })
                .ToList();

            return Ok(totalFinal);
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public ActionResult ExportReport()
        {
            MemoryStream stream = new MemoryStream();
            var pageName = Request.Headers["Referer"].ToString()?.Split("/");
            if (pageName.Last() != "RaiseDiscountTrackingReport")
            {
                stream = ExportAllReport(JsonConvert.DeserializeObject<List<ExpenseEndorsementProfitReport>>(FakeSession.Instance.Obj), 1, pageName.Last());
            }
            else
            {
                stream = ExportAllRDTReport(JsonConvert.DeserializeObject<List<RDTResponse>>(FakeSession.Instance.Obj), 1, pageName.Last());
            }

            string fileName = String.Format("{0}.xlsx", pageName.Last().ToString());
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public MemoryStream ExportAllReport(List<ExpenseEndorsementProfitReport> items, byte type, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add(pageName);

                if (pageName == "EnergyProfitReport")
                {
                    using (var range = ws.Cells[1, 1, 1, 2])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[1, 1].Value = "Ay";
                    ws.Cells[1, 2].Value = "Toplam Kâr";

                    ws.Row(1).Style.Font.Bold = true;

                    for (int c = 2; c < items.Count + 2; c++)
                    {
                        ws.Cells[c, 1].Value = items[c - 2].MonthName;

                        ws.Cells[c, 2].Value = items[c - 2].TotalProfit.ToString("C2", new CultureInfo("tr-TR"));
                    }

                    ws.Column(2).Style.Numberformat.Format = "#,##0.00 ₺";

                    var lastRow = ws.Dimension.End.Row;
                    var lastColumn = ws.Dimension.End.Column;

                    using (var range = ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[lastRow + 1, 1].Value = "Toplam:";
                    ws.Cells[lastRow + 1, 2].Formula = String.Format("SUM(B2:B{0})", lastRow);

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.Cells["A1:B" + items.Count + 2].AutoFilter = true;

                    ws.Column(2).PageBreak = true;
                }
                if (pageName == "ExpenseReport")
                {
                    using (var range = ws.Cells[1, 1, 1, 2])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[1, 1].Value = "Ay";
                    ws.Cells[1, 2].Value = "Toplam Miktar";

                    ws.Row(1).Style.Font.Bold = true;

                    for (int c = 2; c < items.Count + 2; c++)
                    {
                        ws.Cells[c, 1].Value = items[c - 2].MonthName;

                        ws.Cells[c, 2].Value = items[c - 2].TotalAmount.ToString("C2", new CultureInfo("tr-TR"));
                    }

                    ws.Column(2).Style.Numberformat.Format = "#,##0.00 ₺";

                    var lastRow = ws.Dimension.End.Row;
                    var lastColumn = ws.Dimension.End.Column;

                    using (var range = ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[lastRow + 1, 1].Value = "Toplam:";
                    ws.Cells[lastRow + 1, 2].Formula = String.Format("SUM(B2:B{0})", lastRow);

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.Cells["A1:B" + items.Count + 2].AutoFilter = true;

                    ws.Column(2).PageBreak = true;
                }
                if (pageName == "EndorsementReport")
                {
                    using (var range = ws.Cells[1, 1, 1, 4])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    foreach (var item in items)
                    {
                        item.Percent = (item.TotalProfit * 100) / item.TotalAmount;
                    }

                    ws.Cells[1, 1].Value = "Ay";
                    ws.Cells[1, 2].Value = "Toplam Ciro";
                    ws.Cells[1, 3].Value = "Toplam Kâr";
                    ws.Cells[1, 4].Value = "Ciro-Kâr Yüzdesi";

                    ws.Row(1).Style.Font.Bold = true;

                    for (int c = 2; c < items.Count + 2; c++)
                    {
                        ws.Cells[c, 1].Value = items[c - 2].MonthName;

                        if (items[c - 2].TotalAmountCurrencyName == "₺")
                        {
                            ws.Cells[c, 2].Value = items[c - 2].TotalAmount.ToString("C2", new CultureInfo("tr-TR"));
                        }
                        if (items[c - 2].TotalAmountCurrencyName == "$")
                        {
                            ws.Cells[c, 2].Value = items[c - 2].TotalAmount.ToString("C2", new CultureInfo("en-US"));
                        }
                        if (items[c - 2].TotalAmountCurrencyName == "€")
                        {
                            ws.Cells[c, 2].Value = items[c - 2].TotalAmount.ToString("C2", new CultureInfo("de-DE"));
                        }
                        if (items[c - 2].TotalAmountCurrencyName == "£")
                        {
                            ws.Cells[c, 2].Value = items[c - 2].TotalAmount.ToString("C2", new CultureInfo("en-GB"));
                        }

                        if (items[c - 2].TotalProfitCurrencyName == "₺")
                        {
                            ws.Cells[c, 3].Value = items[c - 2].TotalProfit.ToString("C2", new CultureInfo("tr-TR"));
                        }
                        if (items[c - 2].TotalProfitCurrencyName == "$")
                        {
                            ws.Cells[c, 3].Value = items[c - 2].TotalProfit.ToString("C2", new CultureInfo("en-US"));
                        }
                        if (items[c - 2].TotalProfitCurrencyName == "€")
                        {
                            ws.Cells[c, 3].Value = items[c - 2].TotalProfit.ToString("C2", new CultureInfo("de-DE"));
                        }
                        if (items[c - 2].TotalProfitCurrencyName == "£")
                        {
                            ws.Cells[c, 3].Value = items[c - 2].TotalProfit.ToString("C2", new CultureInfo("en-GB"));
                        }

                        ws.Cells[c, 4].Value = "% " + Math.Round(items[c - 2].Percent, 2);
                    }

                    ws.Column(2).Style.Numberformat.Format = "#,##0.00 ₺";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00 ₺";

                    var lastRow = ws.Dimension.End.Row;
                    var lastColumn = ws.Dimension.End.Column;

                    using (var range = ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[lastRow + 1, 1].Value = "Toplam:";
                    ws.Cells[lastRow + 1, 2].Formula = String.Format("SUM(B2:B{0})", lastRow);
                    ws.Cells[lastRow + 1, 3].Formula = String.Format("SUM(C2:C{0})", lastRow);

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.Cells["A1:D" + items.Count + 2].AutoFilter = true;

                    ws.Column(4).PageBreak = true;
                }
                if (pageName == "POSReport")
                {
                    using (var range = ws.Cells[1, 1, 1, 4])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[1, 1].Value = "Yıl";
                    ws.Cells[1, 2].Value = "Ay";
                    ws.Cells[1, 3].Value = "Banka/Şube Adı";
                    ws.Cells[1, 4].Value = "Toplam Miktar";

                    ws.Row(1).Style.Font.Bold = true;

                    for (int c = 2; c < items.Count + 2; c++)
                    {
                        ws.Cells[c, 1].Value = items[c - 2].Year;

                        ws.Cells[c, 2].Value = items[c - 2].MonthName;

                        ws.Cells[c, 3].Value = items[c - 2].BankBranchName;

                        ws.Cells[c, 4].Value = items[c - 2].TotalProfit.ToString("C2", new CultureInfo("tr-TR"));
                    }

                    ws.Column(4).Style.Numberformat.Format = "#,##0.00 ₺";

                    var lastRow = ws.Dimension.End.Row;
                    var lastColumn = ws.Dimension.End.Column;

                    using (var range = ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[lastRow + 1, 3].Value = "Toplam:";
                    ws.Cells[lastRow + 1, 4].Formula = String.Format("SUM(D2:D{0})", lastRow);

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.Cells["A1:D" + items.Count + 2].AutoFilter = true;

                    ws.Column(4).PageBreak = true;
                }
                if (pageName == "FuelSaleReport")
                {
                    using (var range = ws.Cells[1, 1, 1, 5])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[1, 1].Value = "Satış Tutarı";
                    ws.Cells[1, 2].Value = "Fark";
                    ws.Cells[1, 3].Value = "Alım-Satım Kâr";
                    ws.Cells[1, 4].Value = "Yakıt Alım Değeri";
                    ws.Cells[1, 5].Value = "Kâr";

                    ws.Row(1).Style.Font.Bold = true;

                    for (int c = 2; c < items.Count + 2; c++)
                    {
                        ws.Cells[c, 1].Value = items[c - 2].TotalAmount.ToString("C2", new CultureInfo("tr-TR"));

                        ws.Cells[c, 2].Value = items[c - 2].DifferenceAmount.ToString("C2", new CultureInfo("tr-TR"));

                        ws.Cells[c, 3].Value = items[c - 2].TotalPurchaseSaleProfit.ToString("C2", new CultureInfo("tr-TR"));

                        ws.Cells[c, 4].Value = items[c - 2].FuelPurchaseAmount.ToString("C2", new CultureInfo("tr-TR"));

                        ws.Cells[c, 5].Value = items[c - 2].TotalProfit.ToString("C2", new CultureInfo("tr-TR"));
                    }

                    ws.Column(1).Style.Numberformat.Format = "#,##0.00 ₺";
                    ws.Column(2).Style.Numberformat.Format = "#,##0.00 ₺";
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00 ₺";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00 ₺";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00 ₺";

                    var lastRow = ws.Dimension.End.Row;
                    var lastColumn = ws.Dimension.End.Column;

                    using (var range = ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[lastRow + 1, 1].Formula = String.Format("SUM(A2:A{0})", lastRow);
                    ws.Cells[lastRow + 1, 2].Formula = String.Format("SUM(B2:B{0})", lastRow);
                    ws.Cells[lastRow + 1, 3].Formula = String.Format("SUM(C2:C{0})", lastRow);
                    ws.Cells[lastRow + 1, 4].Formula = String.Format("SUM(D2:D{0})", lastRow);
                    ws.Cells[lastRow + 1, 5].Formula = String.Format("SUM(E2:E{0})", lastRow);

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.Cells["A1:E" + items.Count + 2].AutoFilter = true;

                    ws.Column(5).PageBreak = true;
                }

                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                ws.PrinterSettings.Scale = 100;

                p.Save();
            }
            AddExportAudit(pageName, HttpContext?.User?.Identity?.Name, _context);
            return stream;
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public MemoryStream ExportAllRDTReport(List<RDTResponse> items, byte type, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add(pageName);

                using (var range = ws.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "Sektör/İş Kolu Adı";
                ws.Cells[1, 2].Value = "Yakıt Türü";
                ws.Cells[1, 3].Value = "Tarih";
                ws.Cells[1, 4].Value = "Miktar (Fark * Kalan Yakıt)";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].SectorName;

                    ws.Cells[c, 2].Value = items[c - 2].FuelTypeName;

                    ws.Cells[c, 3].Value = items[c - 2].Date;

                    ws.Cells[c, 4].Value = items[c - 2].DifferenceAmount.ToString("C2", new CultureInfo("tr-TR"));
                }

                ws.Column(3).Style.Numberformat.Format = "dd/MM/yyyy";
                ws.Column(4).Style.Numberformat.Format = "#,##0.00 ₺";

                var lastRow = ws.Dimension.End.Row;
                var lastColumn = ws.Dimension.End.Column;

                using (var range = ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[lastRow + 1, 3].Value = "Toplam:";
                ws.Cells[lastRow + 1, 4].Formula = String.Format("SUM(D2:D{0})", lastRow);

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells["A1:D" + items.Count + 2].AutoFilter = true;

                ws.Column(4).PageBreak = true;

                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                ws.PrinterSettings.Scale = 100;

                p.Save();
            }
            AddExportAudit(pageName, HttpContext?.User?.Identity?.Name, _context);
            return stream;
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