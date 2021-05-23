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

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public IActionResult EndorsementReport()
        {
            return View();
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public IActionResult EnergyProfitReport()
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
                .Where(i => i.Date.Month >= DateTime.Now.AddMonths(-2).Month)
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
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
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
        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
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

        //[HttpPost]
        //[Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        //public async Task<IActionResult> ProfitReportPost(int year)
        //{
        //    var usedVehicleProfit = await _context.UsedVehicleSales
        //        .Where(e => e.SaleDate.Year == year)
        //        .GroupBy(e => new
        //        {
        //            e.SaleDate.Month,
        //            e.SaleAmountCurrency,
        //            e.PurchaseAmountCurrency
        //        })
        //        .Select(e => new ExpenseEndorsementProfitReport
        //        {
        //            Month = e.Key.Month,
        //            TotalAmount = e.Sum(i => i.SaleAmount) - e.Sum(i => i.PurchaseAmount),
        //            TotalAmountCurrency = e.Key.SaleAmountCurrency
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();

        //    var newVehicleProfit = await _context.NewVehicleSales
        //        .Where(e => e.SaleDate.Year == year)
        //        .GroupBy(e => new
        //        {
        //            e.SaleDate.Month,
        //            e.SaleAmountCurrency,
        //            e.VehicleCost
        //        })
        //        .Select(e => new ExpenseEndorsementProfitReport
        //        {
        //            Month = e.Key.Month,
        //            TotalAmount = e.Sum(i => i.SaleAmount) - e.Sum(i => i.VehicleCost),
        //            TotalAmountCurrency = e.Key.SaleAmountCurrency
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();

        //    foreach (var item in usedVehicleProfit)
        //    {
        //        newVehicleProfit.Add(item);
        //    }

        //    var totalProfit = newVehicleProfit
        //        .GroupBy(e => new
        //        {
        //            e.Month,
        //            e.TotalAmountCurrency
        //        })
        //        .Select(e => new ExpenseEndorsementProfitReport
        //        {
        //            Month = e.Key.Month,
        //            TotalAmount = e.Sum(i => i.TotalAmount),
        //            TotalAmountCurrency = e.Key.TotalAmountCurrency
        //        })
        //        .OrderBy(i => i.Month)
        //        .ToList();

        //    totalProfit = GetAllEnumNamesHelper.GetEnumName(totalProfit);

        //    FakeSession.Instance.Obj = JsonConvert.SerializeObject(totalProfit);

        //    var response = new PaginatedResponse<ExpenseEndorsementProfitReport>
        //    {
        //        Data = totalProfit,
        //        Draw = 1,
        //        RecordsFiltered = 0,
        //        RecordsTotal = 0
        //    };

        //    return Ok(response);
        //}

        //[HttpPost]
        //[Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        //public async Task<IActionResult> ProfitReportTotalPost(int year)
        //{
        //    var usedVehicleProfit = await _context.UsedVehicleSales
        //        .Where(e => e.SaleDate.Year == year)
        //        .GroupBy(e => new
        //        {
        //            e.SaleAmountCurrency,
        //            e.PurchaseAmountCurrency
        //        })
        //        .Select(e => new ExpenseEndorsementProfitReport
        //        {
        //            TotalAmount = e.Sum(i => i.SaleAmount) - e.Sum(i => i.PurchaseAmount),
        //            TotalAmountCurrency = e.Key.SaleAmountCurrency
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();

        //    var newVehicleProfit = await _context.NewVehicleSales
        //        .Where(e => e.SaleDate.Year == year)
        //        .GroupBy(e => new
        //        {
        //            e.SaleAmountCurrency,
        //            e.VehicleCost
        //        })
        //        .Select(e => new ExpenseEndorsementProfitReport
        //        {
        //            TotalAmount = e.Sum(i => i.SaleAmount) - e.Sum(i => i.VehicleCost),
        //            TotalAmountCurrency = e.Key.SaleAmountCurrency
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();

        //    foreach (var item in usedVehicleProfit)
        //    {
        //        newVehicleProfit.Add(item);
        //    }

        //    var totalProfit = newVehicleProfit
        //        .GroupBy(e => new
        //        {
        //            e.TotalAmountCurrency
        //        })
        //        .Select(e => new ExpenseEndorsementProfitReport
        //        {
        //            TotalAmount = e.Sum(i => i.TotalAmount),
        //            TotalAmountCurrency = e.Key.TotalAmountCurrency
        //        })
        //        .OrderBy(i => i.Month)
        //        .ToList();

        //    totalProfit = GetAllEnumNamesHelper.GetEnumName(totalProfit);

        //    return Ok(totalProfit);
        //}

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public ActionResult ExportReport()
        {
            var pageName = Request.Headers["Referer"].ToString()?.Split("/");
            var stream = ExportAllReport(JsonConvert.DeserializeObject<List<ExpenseEndorsementProfitReport>>(FakeSession.Instance.Obj), 1, pageName.Last());
            string fileName = String.Format("{0}.xlsx", pageName.Last().ToString());
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

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