using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using ExpenseManagement.Helpers;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
    public class MyVehiclesController : Controller
    {
        private readonly ExpenseContext _context;

        public MyVehiclesController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var requestFormData = Request.Form;

            var myVehicle = await _context.MyVehicles
                .Include(d => d.CarModel)
                .Include(d => d.CarModel.CarBrand)
                .AsNoTracking()
                .ToListAsync();

            List<MyVehicles> listItems = ProcessCollection(myVehicle, requestFormData);

            var response = new PaginatedResponse<MyVehicles>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = myVehicle.Count,
                RecordsTotal = myVehicle.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MyVehicles myVehicles)
        {
            if (ModelState.IsValid)
            {
                myVehicles.CarModelId = _context.CarModels.FirstOrDefault(x => x.Name == myVehicles.CarModel.Name).Id;

                _context.Add(myVehicles);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Araç Kaydı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Araç Kaydı Oluşturulurken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewBag.CarBrands = new SelectList(_context.CarBrands.OrderBy(x => x.Name), "Id", "Name");
            ViewBag.CarModels = new SelectList(_context.CarModels.OrderBy(x => x.Name), "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MyVehicles myVehicles)
        {
            var myVehicle = await _context.MyVehicles.FindAsync(id);

            if (myVehicle != null)
            {
                if (ModelState.IsValid)
                {
                    myVehicle.CarModelId = _context.CarModels.FirstOrDefault(x => x.Name == myVehicles.CarModel.Name).Id;
                    myVehicle.InspectionDate = myVehicles.InspectionDate;
                    myVehicle.KaskoAmount = myVehicles.KaskoAmount;
                    myVehicle.ModelYear = myVehicles.ModelYear;
                    myVehicle.TrafficInsuranceAmount = myVehicles.TrafficInsuranceAmount;
                    myVehicle.LicencePlate = myVehicles.LicencePlate;
                    myVehicle.CurrentValue = myVehicles.CurrentValue;

                    _context.Update(myVehicle);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Araç Kaydı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Araç Kaydı Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var myVehicles = await _context.MyVehicles
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (myVehicles == null)
            {
                return View("Error");
            }

            return Ok(myVehicles);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var myVehicles = await _context.MyVehicles
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (myVehicles == null)
            {
                return View("Error");
            }

            return View(myVehicles);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myVehicles = await _context.MyVehicles.FindAsync(id);
            _context.MyVehicles.Remove(myVehicles);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Araç Silinmiştir!" });
        }

        [HttpPost]
        public async Task<IActionResult> FinishingInspectionDate()
        {
            var myVehicles = await _context.MyVehicles
                .Where(m => m.InspectionDate.AddDays(-15) <= DateTime.Now)
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .ToListAsync();

            var user = await _context.Users.FindAsync(GetLoggedUserId());

            if (user.FinishingInspectionClickDate.Value.Date != DateTime.Now.Date)
            {
                if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Banaz")
                {
                    return Ok(myVehicles);
                }
            }
            return Ok();
        }

        public string GetLoggedUserId()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetLoggedUserRole()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }
    }
}