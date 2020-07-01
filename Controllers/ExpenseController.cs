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
    public class ExpenseController : Controller
    {
        private readonly ExpenseContext _context;

        public ExpenseController(ExpenseContext context)
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

            var expenseContext = await _context.Expenses
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

            List<Expenses> listItems = ProcessCollection(expenseContext, requestFormData);

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

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || expenses.CreatedBy == GetLoggedUserId())
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

            var expenses = await _context.Expenses
                .Include(e => e.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);

            expenses = GetAllEnumNamesHelper.GetEnumName(expenses);

            if (expenses == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || expenses.CreatedBy == GetLoggedUserId())
            {
                return View(expenses);
            }
            return View("AccessDenied");
        }

        public IActionResult Create()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,SectorId,Date,Definition,Amount,AmountCurrency,TAX,TAXCurrency")] Expenses expenses)
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

                        expenses.InvoiceImage = ms.ToArray();
                        expenses.InvoiceImageFormat = Request.Form.Files.First().ContentType;
                    }
                    else
                    {
                        return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                    }
                }

                expenses.State = (int)StateEnum.Active;
                expenses.CreatedAt = DateTime.Now;
                expenses.CreatedBy = GetLoggedUserId();

                _context.Add(expenses);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Gider Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Gider Kaydedilirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var expenses = await _context.Expenses.FindAsync(id);
            expenses = GetAllEnumNamesHelper.GetEnumName(expenses);

            if (expenses == null)
            {
                return View("Error");
            }
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name", expenses.SectorId);

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || expenses.CreatedBy == GetLoggedUserId())
            {
                return View(expenses);
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectorId,Date,Definition,Amount,AmountCurrency,TAX,TAXCurrency")] Expenses expenses)
        {
            var expense = await _context.Expenses.FindAsync(id);

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

                            expense.InvoiceImage = ms.ToArray();
                            expense.InvoiceImageFormat = Request.Form.Files.First().ContentType;
                        }
                        else
                        {
                            return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                        }
                    }

                    expense.SectorId = expenses.SectorId;
                    expense.Date = expenses.Date;
                    expense.Definition = expenses.Definition;
                    expense.Amount = expenses.Amount;
                    expense.AmountCurrency = expenses.AmountCurrency;
                    expense.TAX = expenses.TAX;
                    expense.TAXCurrency = expenses.TAXCurrency;

                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Gider Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Gider Güncellenirken Bir Hata Oluştu!");
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

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || expenses.CreatedBy == GetLoggedUserId())
            {
                return View(expenses);
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var expenses = await _context.Expenses.FindAsync(id);
            expenses.InvoiceImage = null;
            expenses.InvoiceImageFormat = null;
            _context.Expenses.Update(expenses);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Görüntü Silinmiştir!" });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expenses = await _context.Expenses.FindAsync(id);
            _context.Expenses.Remove(expenses);
            await _context.SaveChangesAsync();
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

        private bool ExpensesExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
