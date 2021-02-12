using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseManagement.Controllers
{
    [Authorize]
    public class SectorController : Controller
    {
        private readonly ExpenseContext _context;

        public SectorController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Sectors.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name")] Sectors sectors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sectors);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Sektör/İş Kolu Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Sektör/İş Kolu Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var sectors = await _context.Sectors.FindAsync(id);
            if (sectors == null)
            {
                return View("Error");
            }
            return View(sectors);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Sectors sectors)
        {
            var sector = await _context.Sectors.FindAsync(id);

            if (sector != null)
            {
                if (ModelState.IsValid)
                {
                    sector.Name = sectors.Name;

                    _context.Update(sector);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Sektör/İş Kolu Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Sektör/İş Kolu Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var sectors = await _context.Sectors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sectors == null)
            {
                return View("Error");
            }

            return View(sectors);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasAnyIncome = await _context.Incomes
                .FirstOrDefaultAsync(m => m.SectorId == id);

            var hasAnyExpense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.SectorId == id);

            if (hasAnyIncome == null && hasAnyExpense == null)
            {
                var sectors = await _context.Sectors.FindAsync(id);
                _context.Sectors.Remove(sectors);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Sektör/İş Kolu Silinmiştir!" });
            }
            return BadRequest("Bu Sektöre/İş Koluna Ait Kayıtlar Bulunmaktadır!");
        }

        private bool SectorsExists(int id)
        {
            return _context.Sectors.Any(e => e.Id == id);
        }
    }
}
