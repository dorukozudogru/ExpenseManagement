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
    public class RegistrationFeeController : Controller
    {
        private readonly ExpenseContext _context;

        public RegistrationFeeController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.RegistrationFees.OrderBy(x => x.RegistrationFee).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegistrationFees registrationFees)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registrationFees);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Trafik Tescil Bedeli Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Trafik Tescil Bedeli Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var registrationFees = await _context.RegistrationFees.FindAsync(id);
            if (registrationFees == null)
            {
                return View("Error");
            }
            return View(registrationFees);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RegistrationFees registrationFees)
        {
            var registrationFee = await _context.RegistrationFees.FindAsync(id);

            if (registrationFee != null)
            {
                if (ModelState.IsValid)
                {
                    registrationFee.RegistrationFee = registrationFees.RegistrationFee;

                    _context.Update(registrationFee);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Trafik Tescil Bedeli Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Trafik Tescil Bedeli Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var registrationFees = await _context.RegistrationFees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registrationFees == null)
            {
                return View("Error");
            }

            return View(registrationFees);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasAnyVehiclePurchases = await _context.VehiclePurchases
                .FirstOrDefaultAsync(m => m.RegistrationFee.Id == id);

            if (hasAnyVehiclePurchases == null)
            {
                var registrationFees = await _context.RegistrationFees.FindAsync(id);
                _context.RegistrationFees.Remove(registrationFees);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Trafik Tescil Bedeli Silinmiştir!" });
            }
            return BadRequest("Bu Trafik Tescil Bedeline Ait Kayıtlar Bulunmaktadır!");
        }

        private bool RegistrationFeesExists(int id)
        {
            return _context.RegistrationFees.Any(e => e.Id == id);
        }
    }
}
