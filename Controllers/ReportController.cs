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
                .Where(e => e.ExpenseType == expenseType && e.Date >= startDate && e.Date <= finishDate)
                .GroupBy(e => new
                {
                    e.Date.Value.Month,
                    e.AmountCurrency
                })
                .Select(e => new ExpenseReportViewModel
                {
                    Month = e.Key.Month,
                    TotalAmount = e.Sum(i => (double)i.Amount),
                    TotalAmountCurrency = (byte)e.Key.AmountCurrency
                })
                .AsNoTracking()
                .ToListAsync();

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(expenseContext);

            var response = new PaginatedResponse<ExpenseReportViewModel>
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
                .Where(e => e.ExpenseType == expenseType && e.Date >= startDate && e.Date <= finishDate)
                .GroupBy(e => new
                {
                    e.AmountCurrency
                })
                .Select(e => new ExpenseReportViewModel
                {
                    TotalAmount = e.Sum(i => (double)i.Amount),
                    TotalAmountCurrency = (byte)e.Key.AmountCurrency
                })
                .AsNoTracking()
                .ToListAsync();

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            var response = new PaginatedResponse<ExpenseReportViewModel>
            {
                Data = expenseContext,
                Draw = 1,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };

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
                .Select(e => new EndorsementReportViewModel
                {
                    Month = e.Key.Month,
                    TotalAmount = e.Sum(i => i.SaleAmount),
                    TotalAmountCurrency = e.Key.SaleAmountCurrency
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
                .Select(e => new EndorsementReportViewModel
                {
                    Month = e.Key.Month,
                    TotalAmount = e.Sum(i => i.SaleAmount),
                    TotalAmountCurrency = e.Key.SaleAmountCurrency
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
                    e.TotalAmountCurrency
                })
                .Select(e => new EndorsementReportViewModel
                {
                    Month = e.Key.Month,
                    TotalAmount = e.Sum(i => i.TotalAmount),
                    TotalAmountCurrency = e.Key.TotalAmountCurrency
                })
                .OrderBy(i => i.Month)
                .ToList();

            totalSale = GetAllEnumNamesHelper.GetEnumName(totalSale);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(totalSale);

            var response = new PaginatedResponse<EndorsementReportViewModel>
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
                .Select(e => new EndorsementReportViewModel
                {
                    TotalAmount = e.Sum(i => i.SaleAmount),
                    TotalAmountCurrency = e.Key.SaleAmountCurrency
                })
                .AsNoTracking()
                .ToListAsync();

            var newVehicleSale = await _context.NewVehicleSales
                .Where(e => e.SaleDate.Year == year)
                .GroupBy(e => new
                {
                    e.SaleAmountCurrency
                })
                .Select(e => new EndorsementReportViewModel
                {
                    TotalAmount = e.Sum(i => i.SaleAmount),
                    TotalAmountCurrency = e.Key.SaleAmountCurrency
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
                    e.TotalAmountCurrency
                })
                .Select(e => new EndorsementReportViewModel
                {
                    TotalAmount = e.Sum(i => i.TotalAmount),
                    TotalAmountCurrency = e.Key.TotalAmountCurrency
                })
                .OrderBy(i => i.Month)
                .ToList();

            totalSale = GetAllEnumNamesHelper.GetEnumName(totalSale);

            var response = new PaginatedResponse<EndorsementReportViewModel>
            {
                Data = totalSale,
                Draw = 1,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };

            return Ok(totalSale);
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public ActionResult ExportReport()
        {
            var pageName = Request.Headers["Referer"].ToString()?.Split("/");
            var stream = ExportAllReport(JsonConvert.DeserializeObject<List<ExpenseReportViewModel>>(FakeSession.Instance.Obj), 1, pageName.Last());
            string fileName = String.Format("{0}.xlsx", pageName.Last().ToString());
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        public MemoryStream ExportAllReport(List<ExpenseReportViewModel> items, byte type, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add(pageName);

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
                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                ws.PrinterSettings.Scale = 100;

                p.Save();
            }
            AddExportAudit(pageName);
            return stream;
        }

        [Authorize]
        public void AddExportAudit(string pageName)
        {
            Audit audit = new Audit()
            {
                Action = "Exported",
                DateTime = DateTime.Now.ToUniversalTime(),
                KeyValues = "{\"Id\":\"-\"}",
                NewValues = "{\"PageName\":\"" + pageName + "\"}",
                TableName = pageName,
                Username = HttpContext?.User?.Identity?.Name
            };
            _context.Add(audit);
            _context.SaveChanges();
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