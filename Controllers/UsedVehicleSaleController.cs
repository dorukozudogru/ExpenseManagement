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
using ExpenseManagement.Helpers;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;

namespace ExpenseManagement.Controllers
{
    [Authorize]
    public class UsedVehicleSaleController : Controller
    {
        private readonly ExpenseContext _context;

        public UsedVehicleSaleController(ExpenseContext context)
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

            var usedVehicleSale = await _context.UsedVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.SoldSalesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .AsNoTracking()
                .ToListAsync();

            usedVehicleSale = GetAllEnumNamesHelper.GetEnumName(usedVehicleSale);

            List<UsedVehicleSales> listItems = ProcessCollection(usedVehicleSale, requestFormData);

            var response = new PaginatedResponse<UsedVehicleSales>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = usedVehicleSale.Count,
                RecordsTotal = usedVehicleSale.Count
            };

            return Ok(response);
        }

        public ActionResult GetPurchaseDate(string vehiclePurchaseName)
        {
            var chassisNo = vehiclePurchaseName.Split("-").Last().Trim();
            return Json(_context.VehiclePurchases.Where(x => x.Chassis == chassisNo).ToList());
        }

        public IActionResult Create()
        {
            var usedCars = _context.VehiclePurchases
                .Include(cm => cm.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .Select(vp => new VehiclePurchases()
                {
                    Id = vp.Id,
                    FullInfo = vp.CarModel.Name + " - " + vp.Chassis,
                    IsSold = vp.IsSold,
                    IsNew = vp.IsNew,
                    CarBrandName = vp.CarModel.CarBrand.Name
                })
                .Where(vp => vp.IsSold == false && vp.IsNew == false);

            ViewData["PurchasedSalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");
            ViewData["SoldSalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");
            ViewData["VehiclePurchaseId"] = new SelectList(usedCars, "Id", "FullInfo");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsedVehicleSales usedVehicleSales)
        {
            var vehiclePurchases = await _context.VehiclePurchases.FirstOrDefaultAsync(vp => vp.Id == usedVehicleSales.VehiclePurchaseId);

            if (ModelState.IsValid)
            {
                vehiclePurchases.IsSold = true;
                vehiclePurchases.SaleDate = usedVehicleSales.SaleDate;
                vehiclePurchases.SaleAmount = usedVehicleSales.SaleAmount;
                vehiclePurchases.SaleAmountCurrency = usedVehicleSales.SaleAmountCurrency;

                _context.Add(usedVehicleSales);
                _context.Update(vehiclePurchases);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "2. El Araç Satışı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("2. El Araç Satışı Oluşturulurken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var usedVehicleSales = await _context.UsedVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.SoldSalesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            usedVehicleSales = GetAllEnumNamesHelper.GetEnumName(usedVehicleSales);

            if (usedVehicleSales == null)
            {
                return View("Error");
            }

            return View(usedVehicleSales);
        }

        public IActionResult Edit()
        {
            ViewData["PurchasedSalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");
            ViewData["SoldSalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");
            ViewData["VehiclePurchaseId"] = new SelectList(_context.VehiclePurchases.OrderBy(vp => vp.FullInfo), "Id", "Chassis");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var usedVehicleSales = await _context.UsedVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.SoldSalesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usedVehicleSales == null)
            {
                return View("Error");
            }

            return Ok(usedVehicleSales);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UsedVehicleSales usedVehicleSales)
        {
            var usedVehicleSale = await _context.UsedVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.SoldSalesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            var vehiclePurchase = await _context.VehiclePurchases
                .FirstOrDefaultAsync(v => v.Id == usedVehicleSale.VehiclePurchaseId);

            if (usedVehicleSale != null && vehiclePurchase != null)
            {
                if (ModelState.IsValid)
                {
                    vehiclePurchase.SaleDate = usedVehicleSales.SaleDate;
                    vehiclePurchase.SaleAmount = usedVehicleSales.SaleAmount;
                    vehiclePurchase.SaleAmountCurrency = usedVehicleSales.SaleAmountCurrency;

                    usedVehicleSale.LicencePlate = usedVehicleSales.LicencePlate;
                    usedVehicleSale.KM = usedVehicleSales.KM;
                    usedVehicleSale.SaleDate = usedVehicleSales.SaleDate;
                    usedVehicleSale.PurchasedSalesmanId = usedVehicleSales.PurchasedSalesmanId;
                    usedVehicleSale.SoldSalesmanId = usedVehicleSales.SoldSalesmanId;
                    usedVehicleSale.SaleAmount = usedVehicleSales.SaleAmount;
                    usedVehicleSale.SaleAmountCurrency = usedVehicleSales.SaleAmountCurrency;
                    usedVehicleSale.PurchasedSalesmanBonus = usedVehicleSales.PurchasedSalesmanBonus;
                    usedVehicleSale.PurchasedSalesmanBonusCurrency = usedVehicleSales.PurchasedSalesmanBonusCurrency;
                    usedVehicleSale.SoldSalesmanBonus = usedVehicleSales.SoldSalesmanBonus;
                    usedVehicleSale.SoldSalesmanBonusCurrency = usedVehicleSales.SoldSalesmanBonusCurrency;
                    usedVehicleSale.Description = usedVehicleSales.Description;

                    _context.Update(vehiclePurchase);
                    _context.Update(usedVehicleSale);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "2. El Araç Satışı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("2. El Araç Satışı Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var usedVehicleSale = await _context.UsedVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.SoldSalesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            usedVehicleSale = GetAllEnumNamesHelper.GetEnumName(usedVehicleSale);

            if (usedVehicleSale == null)
            {
                return View("Error");
            }

            return View(usedVehicleSale);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usedVehicleSales = await _context.UsedVehicleSales.FindAsync(id);

            if (usedVehicleSales != null)
            {
                var vehiclePurchase = await _context.VehiclePurchases.FirstOrDefaultAsync(vp => vp.Id == usedVehicleSales.VehiclePurchaseId);
                if (vehiclePurchase != null)
                {
                    vehiclePurchase.IsSold = false;
                    vehiclePurchase.SaleDate = null;
                    vehiclePurchase.SaleAmount = null;
                    vehiclePurchase.SaleAmountCurrency = 0;

                    _context.VehiclePurchases.Update(vehiclePurchase);
                }

                _context.UsedVehicleSales.Remove(usedVehicleSales);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "2. El Araç Satışı Silinmiştir!" });
            }
            return BadRequest("2. El Araç Satışı Silinirken Bir Hata Oluştu!");
        }

        private bool UsedVehicleSalesExists(int id)
        {
            return _context.UsedVehicleSales.Any(e => e.Id == id);
        }
    }
}
