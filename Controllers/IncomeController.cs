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

            var expenseContext = await _context.Incomes
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe")
            {
                expenseContext = expenseContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            List<Incomes> listItems = ProcessCollection(expenseContext, requestFormData);

            var response = new PaginatedResponse<Incomes>
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

            var incomes = await _context.Incomes
                .Include(i => i.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);

            incomes = GetAllEnumNamesHelper.GetEnumName(incomes);

            if (incomes == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || incomes.CreatedBy == GetLoggedUserId())
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

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || incomes.CreatedBy == GetLoggedUserId())
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
        public async Task<IActionResult> Create([Bind("Id,SectorId,Date,Definition,Amount,AmountCurrency,TAX,TAXCurrency")] Incomes incomes)
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

                        incomes.InvoiceImage = ms.ToArray();
                        incomes.InvoiceImageFormat = Request.Form.Files.First().ContentType;
                    }
                    else
                    {
                        return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                    }
                }

                incomes.State = (int)StateEnum.Active;
                incomes.CreatedAt = DateTime.Now;
                incomes.CreatedBy = GetLoggedUserId();

                _context.Add(incomes);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Gelir Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Gelir Kaydedilirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var incomes = await _context.Incomes.FindAsync(id);
            incomes = GetAllEnumNamesHelper.GetEnumName(incomes);

            if (incomes == null)
            {
                return View("Error");
            }
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name", incomes.SectorId);

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || incomes.CreatedBy == GetLoggedUserId())
            {
                return View(incomes);
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectorId,Date,Definition,Amount,AmountCurrency,TAX,TAXCurrency")] Incomes incomes)
        {
            var income = await _context.Incomes.FindAsync(id);

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

                            income.InvoiceImage = ms.ToArray();
                            income.InvoiceImageFormat = Request.Form.Files.First().ContentType;
                        }
                        else
                        {
                            return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                        }
                    }

                    income.SectorId = incomes.SectorId;
                    income.Date = incomes.Date;
                    income.Definition = incomes.Definition;
                    income.Amount = incomes.Amount;
                    income.AmountCurrency = incomes.AmountCurrency;
                    income.TAX = incomes.TAX;
                    income.TAXCurrency = incomes.TAXCurrency;

                    _context.Update(income);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Gelir Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Gelir Güncellenirken Bir Hata Oluştu!");
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

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || incomes.CreatedBy == GetLoggedUserId())
            {
                return View(incomes);
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var incomes = await _context.Incomes.FindAsync(id);
            incomes.InvoiceImage = null;
            incomes.InvoiceImageFormat = null;
            _context.Incomes.Update(incomes);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Görüntü Silinmiştir!" });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incomes = await _context.Incomes.FindAsync(id);
            _context.Incomes.Remove(incomes);
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
