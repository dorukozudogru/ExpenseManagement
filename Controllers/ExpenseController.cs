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

namespace ExpenseManagement.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ExpenseContext _context;

        public ExpenseController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expenseContext = await _context.Expenses
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            return View(expenseContext);
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

            return View(expenses);
        }

        public IActionResult Create()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,SectorId,Definition,Amount,AmountCurrency,TAX,TAXCurrency,InvoiceImage,State,ChangedAt,ChangedBy")] Expenses expenses)
        {
            if (ModelState.IsValid)
            {
                expenses.State = (int)StateEnum.Active;
                expenses.ChangedAt = DateTime.Now;
                expenses.ChangedBy = GetLoggedUserId();

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
            return View(expenses);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectorId,Definition,Amount,AmountCurrency,TAX,TAXCurrency,InvoiceImage,State,ChangedAt,ChangedBy")] Expenses expenses)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense != null)
            {
                if (ModelState.IsValid)
                {
                    expense.SectorId = expenses.SectorId;
                    expense.Definition = expenses.Definition;
                    expense.Amount = expenses.Amount;
                    expense.AmountCurrency = expenses.AmountCurrency;
                    expense.TAX = expenses.TAX;
                    expense.TAXCurrency = expenses.TAXCurrency;
                    expense.InvoiceImage = expenses.InvoiceImage;

                    expense.ChangedAt = DateTime.Now;
                    expense.ChangedBy = GetLoggedUserId();

                    _context.Update(expenses);
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

            return View(expenses);
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

        private bool ExpensesExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
