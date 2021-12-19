using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Models;
using ExpenseManagement.Data;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe, Plaza"))]
    public class CarBrandController : Controller
    {
        private readonly ExpenseContext _context;

        public CarBrandController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CarBrands.OrderBy(x => x.Name).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarBrands carBrands)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carBrands);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Marka Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Marka Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var carBrands = await _context.CarBrands.FindAsync(id);
            if (carBrands == null)
            {
                return View("Error");
            }
            return View(carBrands);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarBrands carBrands)
        {
            var carBrand = await _context.CarBrands.FindAsync(id);

            if (carBrand != null)
            {
                if (ModelState.IsValid)
                {
                    carBrand.Name = carBrands.Name;

                    _context.Update(carBrand);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Marka Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Marka Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var carBrands = await _context.CarBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carBrands == null)
            {
                return View("Error");
            }

            return View(carBrands);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasAnyInsurance = await _context.VehiclePurchases
                .FirstOrDefaultAsync(m => m.CarModel.CarBrandId == id);

            if (hasAnyInsurance == null)
            {
                var carBrands = await _context.CarBrands.FindAsync(id);
                _context.CarBrands.Remove(carBrands);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Marka Silinmiştir!" });
            }
            return BadRequest("Bu Markaya Ait Kayıtlar Bulunmaktadır!");
        }

        private bool CarBrandsExists(int id)
        {
            return _context.CarBrands.Any(e => e.Id == id);
        }

        public ActionResult GetCarBrands()
        {
            return Json(_context.CarBrands.OrderBy(x => x.Name).ToList());
        }

        public ActionResult GetCarBrandsForFilters()
        {
            var brand = _context.CarBrands.ToList();
            brand.Add(new CarBrands
            {
                Id = 0,
                Name = ""
            });
            brand = brand.OrderBy(b => b.Id).ToList();
            return Json(brand);
        }
    }
}
