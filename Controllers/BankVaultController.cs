using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using ExpenseManagement.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Muhasebe"))]
    public class BankVaultController : Controller
    {
        private readonly ExpenseContext _context;

        public BankVaultController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expenseContext = await _context.BankVaults
                .Include(b => b.AccountType)
                .Include(c => c.BankBranch)
                .AsNoTracking()
                .ToListAsync();

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            return View(expenseContext);
        }

        public IActionResult Create()
        {
            ViewData["AccountTypeId"] = new SelectList(_context.AccountTypes, "Id", "Name");
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,AccountTypeId,Amount,AmountCurrency,BankBranchId")] BankVaults banks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(banks);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Kasa Kaydı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Kasa Kaydı Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var banks = await _context.BankVaults.FindAsync(id);
            banks = GetAllEnumNamesHelper.GetEnumName(banks);

            if (banks == null)
            {
                return View("Error");
            }
            ViewData["AccountTypeId"] = new SelectList(_context.AccountTypes, "Id", "Name", banks.AccountTypeId);
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name", banks.BankBranchId);
            return View(banks);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountTypeId,Amount,AmountCurrency,BankBranchId")] BankVaults banks)
        {
            var bank = await _context.BankVaults.FindAsync(id);

            if (bank != null)
            {
                if (ModelState.IsValid)
                {
                    bank.AccountTypeId = banks.AccountTypeId;
                    bank.Amount = banks.Amount;
                    bank.AmountCurrency = banks.AmountCurrency;
                    bank.BankBranchId = banks.BankBranchId;

                    _context.Update(bank);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Kasa Kaydı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Kasa Kaydı Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var banks = await _context.BankVaults
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
            var banks = await _context.BankVaults.FindAsync(id);
            _context.BankVaults.Remove(banks);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Kasa Kaydı Silinmiştir!" });
        }

        private bool BanksExists(int id)
        {
            return _context.BankVaults.Any(e => e.Id == id);
        }
    }
}
