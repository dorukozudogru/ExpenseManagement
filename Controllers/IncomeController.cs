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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace ExpenseManagement.Controllers
{
    [Authorize]
    public class IncomeController : Controller
    {
        private readonly ExpenseContext _context;

        public IncomeController(ExpenseContext context)
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
            ViewData["SectorId"] = new SelectList(sectors.OrderBy(s => s.Id), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, int sectorId, string definition, double from, double to, int monthId)
        {
            var requestFormData = Request.Form;

            var incomeContext = await _context.Incomes
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                incomeContext = incomeContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            if (isFiltered != false)
            {
                if (sectorId != 0)
                {
                    incomeContext = incomeContext.Where(e => e.SectorId == sectorId).ToList();
                }
                if (definition != null)
                {
                    incomeContext = incomeContext.Where(e => e.Definition.ToUpper().Contains(definition.ToUpper())).ToList();
                }
                if (from != 0 && to != 0)
                {
                    incomeContext = incomeContext.Where(e => e.Amount >= from && e.Amount <= to).ToList();
                }
                if (monthId != 0)
                {
                    incomeContext = incomeContext.Where(e => e.Date != null && e.Date.Month == monthId).ToList();
                }
            }

            incomeContext = GetAllEnumNamesHelper.GetEnumName(incomeContext);

            List<Incomes> listItems = ProcessCollection(incomeContext, requestFormData);

            var response = new PaginatedResponse<Incomes>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = incomeContext.Count,
                RecordsTotal = incomeContext.Count
            };

            return Ok(response);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var incomes = await _context.Incomes
                .Include(i => i.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);

            incomes = GetAllEnumNamesHelper.GetEnumName(incomes);

            if (incomes == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || incomes.CreatedBy == GetLoggedUserId())
            {
                return View(incomes);
            }
            return View("AccessDenied");
        }

        public async Task<IActionResult> Print(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var incomes = await _context.Incomes
                .Include(e => e.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);

            incomes = GetAllEnumNamesHelper.GetEnumName(incomes);

            if (incomes == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || incomes.CreatedBy == GetLoggedUserId())
            {
                return View(incomes);
            }
            return View("AccessDenied");
        }

        public IActionResult Create()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Incomes incomes)
        {
            IncomeDocuments document = new IncomeDocuments();

            if (ModelState.IsValid)
            {
                incomes.State = (int)StateEnum.Active;
                incomes.CreatedAt = DateTime.Now;
                incomes.CreatedBy = GetLoggedUserId();

                _context.Add(incomes);
                await _context.SaveChangesAsync();

                if (Request.Form.Files.Count != 0)
                {
                    if (Request.Form.Files.First().ContentType.Contains("pdf") || Request.Form.Files.First().ContentType.Contains("image"))
                    {
                        MemoryStream ms = new MemoryStream();
                        Request.Form.Files.First().CopyTo(ms);

                        ms.Close();
                        ms.Dispose();

                        document.IncomeId = incomes.Id;
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
                return Ok(new { Result = true, Message = "Gelir Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Gelir Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors.OrderBy(x => x.Name), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Incomes incomes)
        {
            var income = await _context.Incomes.FindAsync(id);

            var oldDocument = await _context.IncomeDocuments
                .FirstOrDefaultAsync(i => i.IncomeId == id);

            IncomeDocuments newDocument = new IncomeDocuments();

            if (income != null)
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
                                newDocument.IncomeId = income.Id;
                                newDocument.InvoiceImage = ms.ToArray();
                                newDocument.InvoiceImageFormat = Request.Form.Files.First().ContentType;

                                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                _context.Add(newDocument);
                                await _context.SaveChangesAsync();
                            }

                            if (oldDocument != null)
                            {
                                oldDocument.IncomeId = income.Id;
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

                    income.SectorId = incomes.SectorId;
                    income.Definition = incomes.Definition;
                    income.Date = incomes.Date;
                    income.Amount = incomes.Amount;
                    income.AmountCurrency = incomes.AmountCurrency;
                    income.TAX = incomes.TAX;
                    income.TAXCurrency = incomes.TAXCurrency;

                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                    _context.Update(income);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Gelir Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Gelir Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var incomes = await _context.Incomes
                .FirstOrDefaultAsync(m => m.Id == id);
            incomes = GetAllEnumNamesHelper.GetEnumName(incomes);

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || incomes.CreatedBy == GetLoggedUserId())
            {
                return Ok(incomes);
            }

            if (incomes == null)
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

            var document = await _context.IncomeDocuments
                .FirstOrDefaultAsync(m => m.IncomeId == id);

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

            var incomes = await _context.Incomes
                .Include(i => i.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);
            incomes = GetAllEnumNamesHelper.GetEnumName(incomes);

            if (incomes == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || incomes.CreatedBy == GetLoggedUserId())
            {
                return View(incomes);
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var documents = await _context.IncomeDocuments.FirstOrDefaultAsync(ed => ed.IncomeId == id);
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _context.IncomeDocuments.Remove(documents);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Görüntü Silinmiştir!" });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incomes = await _context.Incomes.FindAsync(id);
            _context.Incomes.Remove(incomes);
            await _context.SaveChangesAsync();

            var documents = await _context.IncomeDocuments.FirstOrDefaultAsync(ed => ed.IncomeId == id);
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _context.IncomeDocuments.Remove(documents);
            await _context.SaveChangesAsync();

            return Ok(new { Result = true, Message = "Gelir Silinmiştir!" });
        }

        public string GetLoggedUserId()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetLoggedUserRole()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public ActionResult ExportIncomes()
        {
            var incomeContext = _context.Incomes
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToList();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                incomeContext = incomeContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            var stream = ExportAllIncomes(incomeContext, "Gelirler");
            string fileName = String.Format("{0}.xlsx", "Gelirler");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public MemoryStream ExportAllIncomes(List<Incomes> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            items = GetAllEnumNamesHelper.GetEnumName(items);

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("Gelirler");

                using (var range = ws.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "ID";
                ws.Cells[1, 2].Value = "Sektör/İş Kolu";
                ws.Cells[1, 3].Value = "Gelir Tarihi";
                ws.Cells[1, 4].Value = "Fatura Tanımı";
                ws.Cells[1, 5].Value = "Tutar";
                ws.Cells[1, 6].Value = "KDV";

                ws.Column(3).Style.Numberformat.Format = "dd-mmmm-yyyy";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Id;
                    ws.Cells[c, 2].Value = items[c - 2].Sector.Name;
                    ws.Cells[c, 3].Value = items[c - 2].Date;
                    ws.Cells[c, 4].Value = items[c - 2].Definition;
                    ws.Cells[c, 5].Value = items[c - 2].Amount + " " + items[c - 2].AmountCurrencyName;
                    ws.Cells[c, 6].Value = items[c - 2].TAX + " " + items[c - 2].TAXCurrencyName;

                    ws.Column(5).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].AmountCurrencyName);
                    ws.Column(6).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].TAXCurrencyName);
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
    }
}
