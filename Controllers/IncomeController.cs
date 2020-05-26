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

namespace ExpenseManagement.Controllers
{
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

            return View(incomes);
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

            return View(incomes);
        }

        public IActionResult Create()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,SectorId,Definition,Amount,AmountCurrency,TAX,TAXCurrency")] Incomes incomes)
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
                incomes.ChangedAt = DateTime.Now;
                incomes.ChangedBy = GetLoggedUserId();

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
            return View(incomes);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectorId,Definition,Amount,AmountCurrency,TAX,TAXCurrency")] Incomes incomes)
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
                    income.Definition = incomes.Definition;
                    income.Amount = incomes.Amount;
                    income.AmountCurrency = incomes.AmountCurrency;
                    income.TAX = incomes.TAX;
                    income.TAXCurrency = incomes.TAXCurrency;

                    income.ChangedAt = DateTime.Now;
                    income.ChangedBy = GetLoggedUserId();

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

            return View(incomes);
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

        private bool IncomesExists(int id)
        {
            return _context.Incomes.Any(e => e.Id == id);
        }
    }
}
