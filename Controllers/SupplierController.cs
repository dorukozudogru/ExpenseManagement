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
    public class SupplierController : Controller
    {
        private readonly ExpenseContext _context;

        public SupplierController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Suppliers.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name")] Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suppliers);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Satıcı/Tedarikçi Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Satıcı/Tedarikçi Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var suppliers = await _context.Suppliers.FindAsync(id);
            if (suppliers == null)
            {
                return View("Error");
            }
            return View(suppliers);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Suppliers suppliers)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier != null)
            {
                if (ModelState.IsValid)
                {
                    supplier.Name = suppliers.Name;

                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Satıcı/Tedarikçi Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Satıcı/Tedarikçi Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var suppliers = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suppliers == null)
            {
                return View("Error");
            }

            return View(suppliers);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suppliers = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(suppliers);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Satıcı/Tedarikçi Silinmiştir!" });
        }

        private bool SuppliersExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
