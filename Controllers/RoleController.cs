using System;
using System.Linq;
using System.Threading.Tasks;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ExpenseManagement.Models.ViewModels;


namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;
        private readonly ExpenseContext _context;

        public RoleController(ExpenseContext context, UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
                        role.NormalizedName = roles.Name.ToUpperInvariant();
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

        public async Task<IActionResult> RoleAssign(string id)
        {
            try
            {
                AppIdentityUser user = await _userManager.FindByIdAsync(id);
                TempData["Username"] = user.Email.ToString();
                List<AppIdentityRole> allRoles = _roleManager.Roles.ToList();
                List<string> userRoles = await _userManager.GetRolesAsync(user) as List<string>;
                List<RoleAssignViewModel> assignRoles = new List<RoleAssignViewModel>();
                allRoles.ForEach(role => assignRoles.Add(new RoleAssignViewModel
                {
                    HasAssign = userRoles.Contains(role.Name),
                    RoleId = role.Id,
                    RoleName = role.Name
                }));

                return View(assignRoles);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> RoleAssign(List<RoleAssignViewModel> modelList, string id)
        {
            AppIdentityUser user = await _userManager.FindByIdAsync(id);
            foreach (RoleAssignViewModel role in modelList)
            {
                if (role.HasAssign)
                {
                    if (role.RoleName == "Admin")
                    {
                        user.IsAdmin = true;
                        _context.SaveChanges();
                    }
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
                else
                {
                    if (role.RoleName == "Admin")
                    {
                        user.IsAdmin = false;
                        _context.SaveChanges();
                    }
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }
            return RedirectToAction("Index", "Account");
        }
    }
}
