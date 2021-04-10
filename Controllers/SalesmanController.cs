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
    public class SalesmanController : Controller
    {
        private readonly ExpenseContext _context;

        public SalesmanController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Salesmans.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name")] Salesmans salesmans)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesmans);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Danışman Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Danışman Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var salesmans = await _context.Salesmans.FindAsync(id);
            if (salesmans == null)
            {
                return View("Error");
            }
            return View(salesmans);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Salesmans salesmans)
        {
            var salesman = await _context.Salesmans.FindAsync(id);

            if (salesman != null)
            {
                if (ModelState.IsValid)
                {
                    salesman.Name = salesmans.Name;

                    _context.Update(salesman);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Danışman Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Danışman Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var salesmans = await _context.Salesmans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesmans == null)
            {
                return View("Error");
            }

            return View(salesmans);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasAnyNewSale = await _context.NewVehicleSales
                .FirstOrDefaultAsync(m => m.PurchasedSalesmanId == id || m.SalesmanId == id);

            var hasAnyUsedSale = await _context.UsedVehicleSales
                .FirstOrDefaultAsync(m => m.PurchasedSalesmanId == id || m.SoldSalesmanId == id);

            if (hasAnyNewSale == null && hasAnyUsedSale == null)
            {
                var salesmans = await _context.NewVehicleSales.FindAsync(id);
                _context.NewVehicleSales.Remove(salesmans);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Danışman Silinmiştir!" });
            }
            return BadRequest("Bu Danışmana Ait Kayıtlar Bulunmaktadır!");
        }

        private bool SalesmansExists(int id)
        {
            return _context.Salesmans.Any(e => e.Id == id);
        }
    }
}
