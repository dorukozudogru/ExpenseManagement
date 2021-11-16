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
using static ExpenseManagement.Models.ViewModels.ReportViewModel;

namespace ExpenseManagement.Controllers
{
    [Authorize]
    public class BonusController : Controller
    {
        private readonly ExpenseContext _context;

        public BonusController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var bonusTypes = Enum.GetValues(typeof(Bonuses.BonusTypeEnum)).Cast<Bonuses.BonusTypeEnum>().ToList();
            List<string> types = new List<string>();

            foreach (var item in bonusTypes)
            {
                types.Add(EnumExtensionsHelper.GetDisplayName(item));
            }
            types.Add("");

            ViewData["BonusType"] = new SelectList(types.OrderBy(t => t));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, string bonusType, int year, double from, double to, int monthId)
        {
            var requestFormData = Request.Form;

            var bonusContext = await _context.Bonuses
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                bonusContext = bonusContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            var Id = 999999;

            if (isFiltered != false)
            {
                if (bonusType != null)
                {
                    var bonusTypes = Enum.GetValues(typeof(Bonuses.BonusTypeEnum)).Cast<Bonuses.BonusTypeEnum>().ToList();
                    for (int i = 0; i < bonusTypes.Count; i++)
                    {
                        if (EnumExtensionsHelper.GetDisplayName(bonusTypes[i]).ToString() == bonusType)
                        {
                            Id = i;
                        }
                    }
                    bonusContext = bonusContext.Where(e => e.BonusType == Id).ToList();
                }
                if (year != 0)
                {
                    bonusContext = bonusContext.Where(e => e.Year == year).ToList();
                }
                if (from != 0 && to != 0)
                {
                    bonusContext = bonusContext.Where(e => e.Amount >= from && e.Amount <= to).ToList();
                }
                if (monthId != 0)
                {
                    bonusContext = bonusContext.Where(e => e.Month == monthId).ToList();
                }
            }

            bonusContext = GetAllEnumNamesHelper.GetEnumName(bonusContext);

            List<Bonuses> listItems = ProcessCollection(bonusContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(bonusContext);

            var response = new PaginatedResponse<Bonuses>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = bonusContext.Count,
                RecordsTotal = bonusContext.Count
            };

            return Ok(response);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var bonuses = await _context.Bonuses
                .FirstOrDefaultAsync(m => m.Id == id);

            bonuses = GetAllEnumNamesHelper.GetEnumName(bonuses);

            if (bonuses == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || bonuses.CreatedBy == GetLoggedUserId())
            {
                return View(bonuses);
            }
            return View("AccessDenied");
        }

        public IActionResult Create()
        {
            var bonusTypes = Enum.GetValues(typeof(Bonuses.BonusTypeEnum)).Cast<Bonuses.BonusTypeEnum>().ToList();
            List<string> types = new List<string>();

            foreach (var item in bonusTypes)
            {
                types.Add(EnumExtensionsHelper.GetDisplayName(item));
            }

            ViewData["BonusType"] = new SelectList(types);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Bonuses bonuses)
        {
            BonusDocuments document = new BonusDocuments();

            if (ModelState.IsValid)
            {
                bonuses.CreatedBy = GetLoggedUserId();

                _context.Add(bonuses);
                await _context.SaveChangesAsync();

                if (Request.Form.Files.Count != 0)
                {
                    if (Request.Form.Files.First().ContentType.Contains("pdf") || Request.Form.Files.First().ContentType.Contains("image"))
                    {
                        MemoryStream ms = new MemoryStream();
                        Request.Form.Files.First().CopyTo(ms);

                        ms.Close();
                        ms.Dispose();

                        document.BonusId = bonuses.Id;
                        document.InvoiceImage = ms.ToArray();
                        document.InvoiceImageFormat = Request.Form.Files.First().ContentType;

                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        _context.Add(document);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                    }
                }
                return Ok(new { Result = true, Message = "Prim Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Prim Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Bonuses bonuses)
        {
            var bonus = await _context.Bonuses.FindAsync(id);

            var oldDocument = await _context.BonusDocuments
                .FirstOrDefaultAsync(i => i.BonusId == id);

            BonusDocuments newDocument = new BonusDocuments();

            if (bonus != null)
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
                                newDocument.BonusId = bonus.Id;
                                newDocument.InvoiceImage = ms.ToArray();
                                newDocument.InvoiceImageFormat = Request.Form.Files.First().ContentType;

                                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                _context.Add(newDocument);
                                await _context.SaveChangesAsync();
                            }

                            if (oldDocument != null)
                            {
                                newDocument.BonusId = bonus.Id;
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

                    bonus.Amount = bonuses.Amount;
                    bonus.BonusType = bonuses.BonusType;
                    bonus.Month = bonuses.Month;
                    bonus.Year = bonuses.Year;

                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                    _context.Update(bonus);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Prim Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Prim Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var bonus = await _context.Bonuses
                .FirstOrDefaultAsync(m => m.Id == id);
            bonus = GetAllEnumNamesHelper.GetEnumName(bonus);

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || bonus.CreatedBy == GetLoggedUserId())
            {
                return Ok(bonus);
            }

            if (bonus == null)
            {
                return View("Error");
            }

            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> MonthlyTotalPost()
        {
            var requestFormData = Request.Form;

            var bonuses = await _context.Bonuses
                .GroupBy(i => new
                {
                    i.Year,
                    i.Month
                })
                .Select(i => new GeneralResponse
                {
                    Year = i.Key.Year,
                    Month = (int)i.Key.Month,
                    TotalAmount = i.Sum(x => x.Amount),
                })
                .ToListAsync();

            bonuses = GetAllEnumNamesHelper.GetEnumName(bonuses);

            bonuses = bonuses.OrderByDescending(i => i.Year).ThenByDescending(i => i.Month).ToList();

            List<GeneralResponse> listItems = ProcessCollection(bonuses, requestFormData);

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = bonuses.Count,
                RecordsTotal = bonuses.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetDocument(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var document = await _context.BonusDocuments
                .FirstOrDefaultAsync(m => m.BonusId == id);

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

            var bonus = await _context.Bonuses
                .FirstOrDefaultAsync(m => m.Id == id);
            bonus = GetAllEnumNamesHelper.GetEnumName(bonus);

            if (bonus == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || bonus.CreatedBy == GetLoggedUserId())
            {
                return View(bonus);
            }
            return View("AccessDenied");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bonus = await _context.Bonuses.FindAsync(id);
            _context.Bonuses.Remove(bonus);
            await _context.SaveChangesAsync();

            var documents = await _context.BonusDocuments.FirstOrDefaultAsync(ed => ed.BonusId == id);
            if (documents != null)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                _context.BonusDocuments.Remove(documents);
                await _context.SaveChangesAsync();
            }
            return Ok(new { Result = true, Message = "Prim Silinmiştir!" });
        }

        public async Task<IActionResult> Print(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var documents = await _context.BonusDocuments
                .FirstOrDefaultAsync(m => m.BonusId == id);

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

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var documents = await _context.BonusDocuments.FirstOrDefaultAsync(ed => ed.BonusId == id);
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _context.BonusDocuments.Remove(documents);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Görüntü Silinmiştir!" });
        }

        public ActionResult ExportBonus(int month, int year)
        {
            var bonus = _context.Bonuses
                .Where(i => i.Year == year && i.Month == month)
                .ToList();

            bonus = GetAllEnumNamesHelper.GetEnumName(bonus);

            string monthName = bonus.FirstOrDefault().MonthName;

            var stream = ExportAllBonus(bonus, year + " " + monthName + " Ayı Primleri");
            string fileName = String.Format("{0}.xlsx", year + " " + monthName + " Ayı Primleri");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        public MemoryStream ExportAllBonus(List<Bonuses> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            var _temp = pageName.Split(" ");

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add(_temp[0] + " " + _temp[1]);

                using (var range = ws.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "Yıl";
                ws.Cells[1, 2].Value = "Ay";
                ws.Cells[1, 3].Value = "Toplam Tutar";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Year;
                    ws.Cells[c, 2].Value = items[c - 2].MonthName;
                    ws.Cells[c, 3].Value = items[c - 2].Amount;
                }

                var lastRow = ws.Dimension.End.Row;
                var lastColumn = ws.Dimension.End.Column;

                using (var range = ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[lastRow + 1, 2].Value = "Toplam:";
                ws.Cells[lastRow + 1, 3].Formula = String.Format("SUM(C2:C{0})", lastRow);

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells["A1:C" + items.Count + 2].AutoFilter = true;

                ws.Column(3).PageBreak = true;
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
