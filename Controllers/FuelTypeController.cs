using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Models;
using ExpenseManagement.Data;

namespace ExpenseManagement.Controllers
{
    [Authorize]
    public class FuelTypeController : Controller
    {
        private readonly ExpenseContext _context;

        public FuelTypeController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FuelTypes.OrderBy(x => x.Name).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FuelTypes fuelTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fuelTypes);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Yakıt Türü Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Yakıt Türü Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var fuelTypes = await _context.FuelTypes.FindAsync(id);
            if (fuelTypes == null)
            {
                return View("Error");
            }
            return View(fuelTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FuelTypes fuelTypes)
        {
            var fuelType = await _context.FuelTypes.FindAsync(id);

            if (fuelType != null)
            {
                if (ModelState.IsValid)
                {
                    fuelType.Name = fuelTypes.Name;

                    _context.Update(fuelType);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Yakıt Türü Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Yakıt Türü Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var fuelTypes = await _context.FuelTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fuelTypes == null)
            {
                return View("Error");
            }

            return View(fuelTypes);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasAnyFuelSales = await _context.FuelSales
                .FirstOrDefaultAsync(m => m.FuelType.Id == id);

            if (hasAnyFuelSales == null)
            {
                var fuelTypes = await _context.FuelTypes.FindAsync(id);
                _context.FuelTypes.Remove(fuelTypes);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Yakıt Türü Silinmiştir!" });
            }
            return BadRequest("Bu Yakıt Türüne Ait Kayıtlar Bulunmaktadır!");
        }

        private bool FuelTypesExists(int id)
        {
            return _context.FuelTypes.Any(e => e.Id == id);
        }
    }
}
