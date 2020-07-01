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
    public class EndorsementController : Controller
    {
        private readonly ExpenseContext _context;

        public EndorsementController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expenseContext = await _context.Endorsements
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            return View(expenseContext);
        }

        public IActionResult Create()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,SectorId,Amount,AmountCurrency,Month,Year")] Endorsements endorsements)
        {
            if (ModelState.IsValid)
            {
                _context.Add(endorsements);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Ciro Kaydı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Ciro Kaydı Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var endorsements = await _context.Endorsements.FindAsync(id);
            endorsements = GetAllEnumNamesHelper.GetEnumName(endorsements);
            
            if (endorsements == null)
            {
                return View("Error");
            }
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name", endorsements.SectorId);
            return View(endorsements);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectorId,Amount,AmountCurrency,Month,Year")] Endorsements endorsements)
        {
            var endorsement = await _context.Endorsements.FindAsync(id);

            if (endorsement != null)
            {
                if (ModelState.IsValid)
                {
                    endorsement.SectorId = endorsements.SectorId;
                    endorsement.Amount = endorsements.Amount;
                    endorsement.AmountCurrency = endorsements.AmountCurrency;
                    endorsement.Month = endorsements.Month;
                    endorsement.Year = endorsements.Year;

                    _context.Update(endorsement);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Ciro Kaydı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Ciro Kaydı Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var endorsements = await _context.Endorsements
                .Include(e => e.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);
            endorsements = GetAllEnumNamesHelper.GetEnumName(endorsements);

            if (endorsements == null)
            {
                return View("Error");
            }

            return View(endorsements);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var endorsements = await _context.Endorsements.FindAsync(id);
            _context.Endorsements.Remove(endorsements);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Ciro Kaydı Silinmiştir!" });
        }

        private bool EndorsementsExists(int id)
        {
            return _context.Endorsements.Any(e => e.Id == id);
        }
    }
}
