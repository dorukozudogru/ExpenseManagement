using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using ExpenseManagement.Helpers;

namespace ExpenseManagement.Controllers
{
    public class BankController : Controller
    {
        private readonly ExpenseContext _context;

        public BankController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expenseContext = await _context.Banks
                .Include(b => b.AccountType)
                .AsNoTracking()
                .ToListAsync();

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            return View(expenseContext);
        }

        public IActionResult Create()
        {
            ViewData["AccountTypeId"] = new SelectList(_context.AccountTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,AccountTypeId,Amount,AmountCurrency")] Banks banks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(banks);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Banka Kaydı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Banka Kaydı Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var banks = await _context.Banks.FindAsync(id);
            banks = GetAllEnumNamesHelper.GetEnumName(banks);

            if (banks == null)
            {
                return View("Error");
            }
            ViewData["AccountTypeId"] = new SelectList(_context.AccountTypes, "Id", "Name", banks.AccountTypeId);
            return View(banks);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountTypeId,Amount,AmountCurrency")] Banks banks)
        {
            var bank = await _context.Banks.FindAsync(id);

            if (bank != null)
            {
                if (ModelState.IsValid)
                {
                    bank.AccountTypeId = banks.AccountTypeId;
                    bank.Amount = banks.Amount;
                    bank.AmountCurrency = banks.AmountCurrency;

                    _context.Update(bank);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Banka Kaydı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Banka Kaydı Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var banks = await _context.Banks
                .Include(b => b.AccountType)
                .FirstOrDefaultAsync(m => m.Id == id);
            banks = GetAllEnumNamesHelper.GetEnumName(banks);

            if (banks == null)
            {
                return View("Error");
            }

            return View(banks);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banks = await _context.Banks.FindAsync(id);
            _context.Banks.Remove(banks);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Banka Kaydı Silinmiştir!" });
        }

        private bool BanksExists(int id)
        {
            return _context.Banks.Any(e => e.Id == id);
        }
    }
}
