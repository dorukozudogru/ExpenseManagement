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
using Microsoft.AspNetCore.Authorization;

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
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

        private bool IncomesExists(int id)
        {
            return _context.Incomes.Any(e => e.Id == id);
        }
    }
}
