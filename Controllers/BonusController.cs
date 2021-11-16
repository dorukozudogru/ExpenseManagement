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
