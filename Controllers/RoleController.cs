using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Models;
using ExpenseManagement.Data;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly ExpenseContext _context;

        public RoleController(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppIdentityRole role)
        {
            AppIdentityRole appIdentityRole = new AppIdentityRole()
            {
                Name = role.Name,
                NormalizedName = role.Name.ToUpperInvariant()
            };

            AppIdentityRole tempRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role.Name);

            if (tempRole == null)
            {
                if (!string.IsNullOrEmpty(appIdentityRole.Name))
                {
                    _context.Add(appIdentityRole);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Rol Başarıyla Oluşturulmuştur!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Rol Oluşturulurken Bir Hata Oluştu!");
        }

        //SHOULD SHOW WHO HAS ROLES
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var appIdentityRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appIdentityRole == null)
            {
                return View("Error");
            }

            return View(appIdentityRole);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var appIdentityRole = await _context.Roles.FindAsync(id);
            if (appIdentityRole == null)
            {
                return View("Error");
            }
            return View(appIdentityRole);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, AppIdentityRole roles)
        {
            var role = await _context.Roles.FindAsync(id);
            

            if (role != null)
            {
                if (!string.IsNullOrEmpty(roles.Name))
                {
                    if (ModelState.IsValid)
                    {
                        role.Name = roles.Name;
                        role.NormalizedName = roles.Name;
                        _context.Update(role);
                        await _context.SaveChangesAsync();
                        return Ok(new { Result = true, Message = "Rol Başarıyla Güncellendi!" });
                    }
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Rol Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var appIdentityRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appIdentityRole == null)
            {
                return View("Error");
            }

            return View(appIdentityRole);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var hasAnyUser = await _context.UserRoles
                .FirstOrDefaultAsync(m => m.RoleId == id);

            if (hasAnyUser == null)
            {
                var appIdentityRole = await _context.Roles.FindAsync(id);
                _context.Roles.Remove(appIdentityRole);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Rol Silinmiştir!" });
            }
            return BadRequest("Bu Role Ait Kullanıcı Bulunmaktadır.");
        }

        private bool AppIdentityRoleExists(string id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
