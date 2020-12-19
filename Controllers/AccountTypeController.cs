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
    public class AccountTypeController : Controller
    {
        private readonly ExpenseContext _context;

        public AccountTypeController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.AccountTypes.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name")] AccountTypes accountTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accountTypes);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Hesap Türü Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Hesap Türü Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var accountTypes = await _context.AccountTypes.FindAsync(id);
            if (accountTypes == null)
            {
                return View("Error");
            }
            return View(accountTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] AccountTypes accountTypes)
        {
            var accountType = await _context.AccountTypes.FindAsync(id);

            if (accountType != null)
            {
                if (ModelState.IsValid)
                {
                    accountType.Name = accountTypes.Name;

                    _context.Update(accountType);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Hesap Türü Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Hesap Türü Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var accountTypes = await _context.AccountTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountTypes == null)
            {
                return View("Error");
            }

            return View(accountTypes);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasAnyRecord = await _context.BankVaults
                .FirstOrDefaultAsync(m => m.AccountTypeId == id);

            if (hasAnyRecord == null)
            {
                var accountTypes = await _context.AccountTypes.FindAsync(id);
                _context.AccountTypes.Remove(accountTypes);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Hesap Türü Silinmiştir!" });
            }
            return BadRequest("Bu Hesap Türüne Ait Kayıtlar Bulunmaktadır!");
        }

        private bool AccountTypesExists(int id)
        {
            return _context.AccountTypes.Any(e => e.Id == id);
        }
    }
}
