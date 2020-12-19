using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Muhasebe"))]
    public class BankBranchesController : Controller
    {
        private readonly ExpenseContext _context;

        public BankBranchesController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.BankBranches.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name")] BankBranches bankBranches)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bankBranches);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Banka/Şube Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Banka/Şube Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var bankBranches = await _context.BankBranches.FindAsync(id);
            if (bankBranches == null)
            {
                return View("Error");
            }
            return View(bankBranches);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] BankBranches bankBranches)
        {
            var branch = await _context.BankBranches.FindAsync(id);

            if (branch != null)
            {
                if (ModelState.IsValid)
                {
                    branch.Name = bankBranches.Name;

                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Banka/Şube Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Banka/Şube Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var bankBranches = await _context.BankBranches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankBranches == null)
            {
                return View("Error");
            }

            return View(bankBranches);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasAnyBankVault = await _context.BankVaults
                .FirstOrDefaultAsync(m => m.BankBranchId == id);

            if (hasAnyBankVault == null)
            {
                var bankBranches = await _context.BankBranches.FindAsync(id);
                _context.BankBranches.Remove(bankBranches);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Banka/Şube Kolu Silinmiştir!" });
            }
            return BadRequest("Bu Bankaya/Şubeye Ait Kayıtlar Bulunmaktadır!");
        }

        private bool BankBranchExists(int id)
        {
            return _context.BankBranches.Any(e => e.Id == id);
        }
    }
}
