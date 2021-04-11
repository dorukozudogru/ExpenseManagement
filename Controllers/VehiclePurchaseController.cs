using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using Microsoft.AspNetCore.Authorization;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using ExpenseManagement.Helpers;

namespace ExpenseManagement.Controllers
{
    [Authorize]
    public class VehiclePurchaseController : Controller
    {
        private readonly ExpenseContext _context;

        public VehiclePurchaseController(ExpenseContext context)
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

            var vehiclePurchaseContext = await _context.VehiclePurchases
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .AsNoTracking()
                .ToListAsync();

            vehiclePurchaseContext = GetAllEnumNamesHelper.GetEnumName(vehiclePurchaseContext);

            List<VehiclePurchases> listItems = ProcessCollection(vehiclePurchaseContext, requestFormData);

            var response = new PaginatedResponse<VehiclePurchases>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = vehiclePurchaseContext.Count,
                RecordsTotal = vehiclePurchaseContext.Count
            };

            return Ok(response);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var vehiclePurchases = await _context.VehiclePurchases
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            vehiclePurchases = GetAllEnumNamesHelper.GetEnumName(vehiclePurchases);

            if (vehiclePurchases == null)
            {
                return View("Error");
            }

            return View(vehiclePurchases);
        }

        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehiclePurchases vehiclePurchases)
        {
            if (ModelState.IsValid)
            {
                vehiclePurchases.CarModelId = _context.CarModels.FirstOrDefault(x => x.Name == vehiclePurchases.CarModel.Name).Id;

                _context.Add(vehiclePurchases);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Araç Alımı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Araç Alımı Oluşturulurken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewBag.CarBrands = new SelectList(_context.CarBrands.OrderBy(x => x.Name), "Id", "Name");
            ViewBag.CarModels = new SelectList(_context.CarModels.OrderBy(x => x.Name), "Name", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var vehiclePurchases = await _context.VehiclePurchases
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vehiclePurchases == null)
            {
                return View("Error");
            }

            return Ok(vehiclePurchases);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, VehiclePurchases vehiclePurchases)
        {
            var vehiclePurchase = await _context.VehiclePurchases
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            var newVehicleSale = await _context.NewVehicleSales.FirstOrDefaultAsync(nvs => nvs.VehiclePurchaseId == id);

            if (vehiclePurchase != null)
            {
                if (ModelState.IsValid)
                {
                    if (newVehicleSale != null)
                    {
                        newVehicleSale.PurchaseDate = vehiclePurchases.PurchaseDate;
                        _context.Update(newVehicleSale);
                    }

                    vehiclePurchase.CarModelId = _context.CarModels.FirstOrDefault(x => x.Name == vehiclePurchases.CarModel.Name).Id;
                    vehiclePurchase.IsNew = vehiclePurchases.IsNew;
                    vehiclePurchase.IsSold = vehiclePurchases.IsSold;
                    vehiclePurchase.Chassis = vehiclePurchases.Chassis;
                    vehiclePurchase.PurchaseDate = vehiclePurchases.PurchaseDate;
                    vehiclePurchase.SaleDate = vehiclePurchases.SaleDate;
                    vehiclePurchase.ValorDate = vehiclePurchases.ValorDate;
                    vehiclePurchase.PurchaseAmount = vehiclePurchases.PurchaseAmount;
                    vehiclePurchase.PurchaseAmountCurrency = vehiclePurchases.PurchaseAmountCurrency;

                    vehiclePurchase.OTVPercent = vehiclePurchases.OTVPercent;
                    vehiclePurchase.OTV = vehiclePurchases.OTV;
                    vehiclePurchase.KDV = vehiclePurchases.KDV;
                    vehiclePurchase.RegistrationFee = vehiclePurchases.RegistrationFee;
                    vehiclePurchase.IncludingRegistrationFee = vehiclePurchases.IncludingRegistrationFee;

                    vehiclePurchase.SaleAmount = vehiclePurchases.SaleAmount;
                    vehiclePurchase.SaleAmountCurrency = vehiclePurchases.SaleAmountCurrency;

                    _context.Update(vehiclePurchase);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Araç Alımı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Araç Alımı Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var vehiclePurchases = await _context.VehiclePurchases
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            vehiclePurchases = GetAllEnumNamesHelper.GetEnumName(vehiclePurchases);

            if (vehiclePurchases == null)
            {
                return View("Error");
            }

            return View(vehiclePurchases);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasAnyNewVehicleSales = await _context.NewVehicleSales
                .FirstOrDefaultAsync(m => m.VehiclePurchaseId == id);

            if (hasAnyNewVehicleSales == null)
            {
                var vehiclePurchases = await _context.VehiclePurchases.FindAsync(id);
                _context.VehiclePurchases.Remove(vehiclePurchases);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Araç Alımı Silinmiştir!" });
            }
            return BadRequest("Araç Satışı Bulunduğundan Dolayı Silinemez!");
        }

        [HttpPost]
        public async Task<IActionResult> FinishingValorsPost()
        {
            var requestFormData = Request.Form;

            var vehiclePurchaseContext = await _context.VehiclePurchases
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .Where(vp => vp.ValorDate != null)
                .AsNoTracking()
                .ToListAsync();

            List<VehiclePurchases> listItems = ProcessCollection(vehiclePurchaseContext, requestFormData);

            foreach (var item in listItems)
            {
                item.ValorEndDate = item.PurchaseDate.AddDays((double)item.ValorDate);
            }

            listItems = listItems.Where(li => li.ValorEndDate.AddDays(-2) <= DateTime.Now.Date && li.ValorEndDate >= DateTime.Now.Date).ToList();

            var response = new PaginatedResponse<VehiclePurchases>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = vehiclePurchaseContext.Count,
                RecordsTotal = vehiclePurchaseContext.Count
            };

            ViewBag.NotificationCount = listItems.Count;

            return Ok(response);
        }

        private bool VehiclePurchasesExists(int id)
        {
            return _context.VehiclePurchases.Any(e => e.Id == id);
        }
    }
}
