using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Models;
using ExpenseManagement.Data;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = "Admin, Banaz, Muhasebe, Plaza")]
    public class LetterOfGuaranteeCostController : Controller
    {
        private readonly ExpenseContext _context;

        public LetterOfGuaranteeCostController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.LetterOfGuaranteeCosts.OrderBy(x => x.Cost).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LetterOfGuaranteeCosts letterOfGuaranteeCost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(letterOfGuaranteeCost);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Masraf Yüzdesi Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Masraf Yüzdesi Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var letterOfGuaranteeCost = await _context.LetterOfGuaranteeCosts.FindAsync(id);
            if (letterOfGuaranteeCost == null)
            {
                return View("Error");
            }
            return View(letterOfGuaranteeCost);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, LetterOfGuaranteeCosts letterOfGuaranteeCosts)
        {
            var letterOfGuaranteeCost = await _context.LetterOfGuaranteeCosts.FindAsync(id);

            if (letterOfGuaranteeCost != null)
            {
                if (ModelState.IsValid)
                {
                    letterOfGuaranteeCost.Cost = letterOfGuaranteeCosts.Cost;

                    _context.Update(letterOfGuaranteeCost);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Masraf Yüzdesi Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Masraf Yüzdesi Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var letterOfGuaranteeCost = await _context.LetterOfGuaranteeCosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (letterOfGuaranteeCost == null)
            {
                return View("Error");
            }

            return View(letterOfGuaranteeCost);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var hasAnyLetterOfGuarantee = await _context.LetterOfGuarantee
            //    .FirstOrDefaultAsync(m => m.LetterOfGuaranteeCost.Id == id);

            //if (hasAnyLetterOfGuarantee == null)
            //{
                var letterOfGuaranteeCost = await _context.LetterOfGuaranteeCosts.FindAsync(id);
                _context.LetterOfGuaranteeCosts.Remove(letterOfGuaranteeCost);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Masraf Yüzdesi Silinmiştir!" });
            //}
            //return BadRequest("Bu Masraf Yüzdesine Ait Kayıtlar Bulunmaktadır!");
        }

        private bool LetterOfGuaranteeCostExists(int id)
        {
            return _context.LetterOfGuaranteeCosts.Any(e => e.Id == id);
        }
    }
}
