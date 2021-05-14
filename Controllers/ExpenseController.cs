using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using ExpenseManagement.Helpers;
using System;
using static ExpenseManagement.Models.Expenses;
using System.Security.Claims;
using System.IO;
using System.Collections.Generic;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using static ExpenseManagement.Helpers.AddExportAuditHelper;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using ExpenseManagement.Models.ViewModels;
using Newtonsoft.Json;

namespace ExpenseManagement.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ExpenseContext _context;

        public ExpenseController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sectors = _context.Sectors.ToList();
            sectors.Add(new Sectors
            {
                Id = 0,
                Name = ""
            });

            var expenseTypes = Enum.GetValues(typeof(Expenses.ExpenseTypeEnum)).Cast<Expenses.ExpenseTypeEnum>().ToList();
            List<string> types = new List<string>();

            foreach (var item in expenseTypes)
            {
                if (EnumExtensionsHelper.GetDisplayName(item) == "Maaş")
                {
                    types.Add("");
                }
                else
                {
                    types.Add(EnumExtensionsHelper.GetDisplayName(item));
                }
            }

            ViewData["SectorId"] = new SelectList(sectors.OrderBy(s => s.Id), "Id", "Name");
            ViewData["ExpenseType"] = new SelectList(types.OrderBy(t => t));
            return View();
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public IActionResult Salary()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public IActionResult Paylist()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public async Task<IActionResult> SalaryPost()
        {
            var requestFormData = Request.Form;

            var expenseContext = await _context.Expenses
                .Where(e => e.ExpenseType == 2)
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                expenseContext = expenseContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            List<Expenses> listItems = ProcessCollection(expenseContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(expenseContext);

            var response = new PaginatedResponse<Expenses>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = expenseContext.Count,
                RecordsTotal = expenseContext.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, string expenseType, string supplier, int sectorId, string definition, double from, double to, int monthId)
        {
            var requestFormData = Request.Form;

            var expenseContext = await _context.Expenses
                .Where(e => e.ExpenseType != 2)
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                expenseContext = expenseContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            var Id = 999999;

            if (isFiltered != false)
            {
                if (expenseType != null)
                {
                    var expenseTypes = Enum.GetValues(typeof(Expenses.ExpenseTypeEnum)).Cast<Expenses.ExpenseTypeEnum>().ToList();
                    for (int i = 0; i < expenseTypes.Count; i++)
                    {
                        if (EnumExtensionsHelper.GetDisplayName(expenseTypes[i]).ToString() == expenseType)
                        {
                            Id = i;
                        }
                    }
                    expenseContext = expenseContext.Where(e => e.ExpenseType == Id).ToList();
                }
                if (supplier != null)
                {
                    expenseContext = expenseContext.Where(e => e.SupplierDef != null && e.SupplierDef.ToUpper().Contains(supplier.ToUpper())).ToList();
                }
                if (sectorId != 0)
                {
                    expenseContext = expenseContext.Where(e => e.SectorId == sectorId).ToList();
                }
                if (definition != null)
                {
                    expenseContext = expenseContext.Where(e => e.Definition.ToUpper().Contains(definition.ToUpper())).ToList();
                }
                if (from != 0 && to != 0)
                {
                    expenseContext = expenseContext.Where(e => e.Amount >= from && e.Amount <= to).ToList();
                }
                if (monthId != 0)
                {
                    expenseContext = expenseContext.Where(e => e.Date != null && e.Date.Value.Month == monthId).ToList();
                }
            }

            foreach (var item in expenseContext)
            {
                if (item.Sector != null)
                {
                    item.SectorName = item.Sector.Name;
                }
                if (item.Sector == null)
                {
                    item.SectorName = "EMPTY";
                }
            }

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            List<Expenses> listItems = ProcessCollection(expenseContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(expenseContext);

            var response = new PaginatedResponse<Expenses>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = expenseContext.Count,
                RecordsTotal = expenseContext.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> WeeklyPaylistPost()
        {
            var requestFormData = Request.Form;

            DateTime baseDate = DateTime.Today;
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek + 1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);

            if (baseDate.DayOfWeek.ToString() == "Sunday")
            {
                thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek - 6);
                thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            }

            var expenseContext = await _context.Expenses
                .Where(e => e.ExpenseType != 2 &&
                            e.LastPaymentDate != null &&
                            e.LastPaymentDate >= thisWeekStart &&
                            e.LastPaymentDate <= thisWeekEnd)
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                expenseContext = expenseContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            List<Expenses> listItems = ProcessCollection(expenseContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(expenseContext);

            var response = new PaginatedResponse<Expenses>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = expenseContext.Count,
                RecordsTotal = expenseContext.Count
            };

            return Ok(response);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var expenses = await _context.Expenses
                .Include(e => e.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);

            expenses = GetAllEnumNamesHelper.GetEnumName(expenses);

            if (expenses == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || expenses.CreatedBy == GetLoggedUserId())
            {
                return View(expenses);
            }
            return View("AccessDenied");
        }

        public async Task<IActionResult> Print(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var documents = await _context.ExpenseDocuments
                .FirstOrDefaultAsync(m => m.ExpenseId == id);

            if (documents == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz")
            {
                return View(documents);
            }
            return View("AccessDenied");
        }

        public IActionResult Create()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expenses expenses)
        {
            ExpenseDocuments document = new ExpenseDocuments();

            if (ModelState.IsValid)
            {
                expenses.State = (int)StateEnum.Active;
                expenses.CreatedAt = DateTime.Now;
                expenses.CreatedBy = GetLoggedUserId();

                _context.Add(expenses);
                await _context.SaveChangesAsync();

                if (Request.Form.Files.Count != 0)
                {
                    if (Request.Form.Files.First().ContentType.Contains("pdf") || Request.Form.Files.First().ContentType.Contains("image"))
                    {
                        MemoryStream ms = new MemoryStream();
                        Request.Form.Files.First().CopyTo(ms);

                        ms.Close();
                        ms.Dispose();

                        document.ExpenseId = expenses.Id;
                        document.InvoiceImage = ms.ToArray();
                        document.InvoiceImageFormat = Request.Form.Files.First().ContentType;

                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        _context.Add(document);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                    }
                }
                return Ok(new { Result = true, Message = "Gider Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Gider Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Expenses expenses)
        {
            var expense = await _context.Expenses.FindAsync(id);

            var oldDocument = await _context.ExpenseDocuments
                .FirstOrDefaultAsync(i => i.ExpenseId == id);

            ExpenseDocuments newDocument = new ExpenseDocuments();

            if (expense != null)
            {
                if (ModelState.IsValid)
                {
                    if (Request.Form.Files.Count != 0)
                    {
                        if (Request.Form.Files.First().ContentType.Contains("pdf") || Request.Form.Files.First().ContentType.Contains("image"))
                        {
                            MemoryStream ms = new MemoryStream();
                            Request.Form.Files.First().CopyTo(ms);

                            ms.Close();
                            ms.Dispose();

                            if (oldDocument == null)
                            {
                                newDocument.ExpenseId = expense.Id;
                                newDocument.InvoiceImage = ms.ToArray();
                                newDocument.InvoiceImageFormat = Request.Form.Files.First().ContentType;

                                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                _context.Add(newDocument);
                                await _context.SaveChangesAsync();
                            }

                            if (oldDocument != null)
                            {
                                oldDocument.ExpenseId = expense.Id;
                                oldDocument.InvoiceImage = ms.ToArray();
                                oldDocument.InvoiceImageFormat = Request.Form.Files.First().ContentType;

                                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                _context.Update(oldDocument);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                        }
                    }

                    expense.ExpenseType = expenses.ExpenseType;
                    expense.SectorId = expenses.SectorId;
                    expense.SupplierDef = expenses.SupplierDef;
                    expense.Definition = expenses.Definition;
                    expense.Date = expenses.Date;
                    expense.LastPaymentDate = expenses.LastPaymentDate;
                    expense.Amount = expenses.Amount;
                    expense.AmountCurrency = expenses.AmountCurrency;
                    expense.TAX = expenses.TAX;
                    expense.TAXCurrency = expenses.TAXCurrency;

                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Gider Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Gider Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            expense = GetAllEnumNamesHelper.GetEnumName(expense);

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || expense.CreatedBy == GetLoggedUserId())
            {
                return Ok(expense);
            }

            if (expense == null)
            {
                return View("Error");
            }

            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> GetDocument(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var document = await _context.ExpenseDocuments
                .FirstOrDefaultAsync(m => m.ExpenseId == id);

            if (document == null)
            {
                return View("Error");
            }

            return Ok(document);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var expenses = await _context.Expenses
                .Include(e => e.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);
            expenses = GetAllEnumNamesHelper.GetEnumName(expenses);

            if (expenses == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || expenses.CreatedBy == GetLoggedUserId())
            {
                return View(expenses);
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var documents = await _context.ExpenseDocuments.FirstOrDefaultAsync(ed => ed.ExpenseId == id);
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _context.ExpenseDocuments.Remove(documents);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Görüntü Silinmiştir!" });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expenses = await _context.Expenses.FindAsync(id);
            _context.Expenses.Remove(expenses);
            await _context.SaveChangesAsync();

            var documents = await _context.ExpenseDocuments.FirstOrDefaultAsync(ed => ed.ExpenseId == id);
            if (documents != null)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                _context.ExpenseDocuments.Remove(documents);
                await _context.SaveChangesAsync();
            }
            return Ok(new { Result = true, Message = "Gider Silinmiştir!" });
        }

        public string GetLoggedUserId()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetLoggedUserRole()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }

        #region ExportPaylist
        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public ActionResult ExportPaylist()
        {
            var stream = ExportAllPaylist(JsonConvert.DeserializeObject<List<Expenses>>(FakeSession.Instance.Obj), "Haftalık Ödeme Listesi");
            string fileName = String.Format("{0}.xlsx", "Haftalık Ödeme Listesi");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public MemoryStream ExportAllPaylist(List<Expenses> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            items = GetAllEnumNamesHelper.GetEnumName(items);

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("Ödemeler");

                using (var range = ws.Cells[1, 1, 1, 9])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "ID";
                ws.Cells[1, 2].Value = "Gider Türü";
                ws.Cells[1, 3].Value = "Sektör/İş Kolu";
                ws.Cells[1, 4].Value = "Satıcı/Tedariçi";
                ws.Cells[1, 5].Value = "Gider Tarihi";
                ws.Cells[1, 6].Value = "Son Ödeme Tarihi";
                ws.Cells[1, 7].Value = "Gider Tanımı";
                ws.Cells[1, 8].Value = "Tutar";
                ws.Cells[1, 9].Value = "KDV";

                ws.Column(5).Style.Numberformat.Format = "dd-mmmm-yyyy";
                ws.Column(6).Style.Numberformat.Format = "dd-mmmm-yyyy";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Id;
                    ws.Cells[c, 2].Value = items[c - 2].ExpenseTypeName;
                    ws.Cells[c, 3].Value = items[c - 2].Sector.Name;
                    ws.Cells[c, 4].Value = items[c - 2].SupplierDef;
                    ws.Cells[c, 5].Value = items[c - 2].Date;
                    ws.Cells[c, 6].Value = items[c - 2].LastPaymentDate;
                    ws.Cells[c, 7].Value = items[c - 2].Definition;
                    ws.Cells[c, 8].Value = items[c - 2].Amount + " " + items[c - 2].AmountCurrencyName;
                    ws.Cells[c, 9].Value = items[c - 2].TAX + " " + items[c - 2].TAXCurrencyName;

                    ws.Column(8).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].AmountCurrencyName);
                    ws.Column(9).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].TAXCurrencyName);
                }

                //var lastRow = ws.Dimension.End.Row;
                //var lastColumn = ws.Dimension.End.Column;

                //ws.Cells[lastRow + 1, 7].Value = "Toplam:";
                //ws.Cells[lastRow + 1, 8].Formula = String.Format("SUM(H2:H{0})", lastRow);
                //ws.Cells[lastRow + 1, 9].Formula = String.Format("SUM(I2:I{0})", lastRow);

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells["A1:I" + items.Count + 2].AutoFilter = true;

                ws.Column(9).PageBreak = true;
                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                ws.PrinterSettings.Scale = 100;

                p.Save();
            }
            AddExportAudit(pageName, HttpContext?.User?.Identity?.Name, _context);
            return stream;
        }
        #endregion

        #region ExportSalary
        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public ActionResult ExportSalary()
        {
            var stream = ExportAllSalary(JsonConvert.DeserializeObject<List<Expenses>>(FakeSession.Instance.Obj), "Maaşlar");
            string fileName = String.Format("{0}.xlsx", "Maaşlar");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public MemoryStream ExportAllSalary(List<Expenses> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            items = GetAllEnumNamesHelper.GetEnumName(items);

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("Maaşlar");

                using (var range = ws.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "ID";
                ws.Cells[1, 2].Value = "Gider Türü";
                ws.Cells[1, 3].Value = "Sektör/İş Kolu";
                ws.Cells[1, 4].Value = "Ay (Maaş)";
                ws.Cells[1, 5].Value = "Gider Tanımı";
                ws.Cells[1, 6].Value = "Net Tutar";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Id;
                    ws.Cells[c, 2].Value = items[c - 2].ExpenseTypeName;
                    ws.Cells[c, 3].Value = items[c - 2].Sector.Name;
                    ws.Cells[c, 4].Value = items[c - 2].MonthName;
                    ws.Cells[c, 5].Value = items[c - 2].Definition;
                    ws.Cells[c, 6].Value = items[c - 2].SalaryAmount + " " + items[c - 2].SalaryAmountCurrencyName;

                    ws.Column(6).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].SalaryAmountCurrencyName);
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells["A1:F" + items.Count + 2].AutoFilter = true;

                ws.Column(6).PageBreak = true;
                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                ws.PrinterSettings.Scale = 100;

                p.Save();
            }
            AddExportAudit(pageName, HttpContext?.User?.Identity?.Name, _context);
            return stream;
        }
        #endregion

        private bool ExpensesExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
