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
    public class NewVehicleSaleController : Controller
    {
        private readonly ExpenseContext _context;

        public NewVehicleSaleController(ExpenseContext context)
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

            var newVehicleSale = await _context.NewVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.Salesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .AsNoTracking()
                .ToListAsync();

            newVehicleSale = GetAllEnumNamesHelper.GetEnumName(newVehicleSale);

            List<NewVehicleSales> listItems = ProcessCollection(newVehicleSale, requestFormData);

            var response = new PaginatedResponse<NewVehicleSales>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = newVehicleSale.Count,
                RecordsTotal = newVehicleSale.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            ViewData["SalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");
            ViewData["PurchasedSalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");

            var newCars = _context.VehiclePurchases
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
                .Where(vp => vp.IsSold == false && vp.IsNew == true && vp.CarBrandName == "Skoda");

            ViewData["VehiclePurchaseId"] = new SelectList(newCars, "Id", "FullInfo");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewVehicleSales newVehicleSales)
        {
            var vehiclePurchases = await _context.VehiclePurchases.FirstOrDefaultAsync(vp => vp.Id == newVehicleSales.VehiclePurchaseId);

            if (ModelState.IsValid)
            {
                vehiclePurchases.IsSold = true;
                vehiclePurchases.SaleDate = newVehicleSales.SaleDate;
                vehiclePurchases.SaleAmount = newVehicleSales.SaleAmount;
                vehiclePurchases.SaleAmountCurrency = newVehicleSales.SaleAmountCurrency;

                _context.Add(newVehicleSales);
                _context.Update(vehiclePurchases);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Sıfır Araç Satışı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Sıfır Araç Satışı Oluşturulurken Bir Hata Oluştu!");
        }

        public ActionResult GetPurchaseDate(string vehiclePurchaseName)
        {
            var chassisNo = vehiclePurchaseName.Split("-").Last().Trim();
            return Json(_context.VehiclePurchases.Where(x => x.Chassis == chassisNo).ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var newVehicleSales = await _context.NewVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.Salesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            newVehicleSales = GetAllEnumNamesHelper.GetEnumName(newVehicleSales);

            if (newVehicleSales == null)
            {
                return View("Error");
            }

            return View(newVehicleSales);
        }

        public IActionResult Edit()
        {
            ViewData["SalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");
            ViewData["PurchasedSalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");
            ViewData["VehiclePurchaseCarBrandId"] = new SelectList(_context.CarBrands, "Id", "Name");
            ViewData["VehiclePurchaseCarModelId"] = new SelectList(_context.CarModels, "Id", "Name");
            ViewData["VehiclePurchaseId"] = new SelectList(_context.VehiclePurchases.OrderBy(vp => vp.FullInfo), "Id", "FullInfo");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var newVehicleSales = await _context.NewVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.Salesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (newVehicleSales == null)
            {
                return View("Error");
            }

            return Ok(newVehicleSales);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewVehicleSales newVehicleSales)
        {
            var newVehicleSale = await _context.NewVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.Salesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            var vehiclePurchase = await _context.VehiclePurchases
                .FirstOrDefaultAsync(v => v.Id == newVehicleSale.VehiclePurchaseId);

            if (newVehicleSale != null && vehiclePurchase != null)
            {
                if (ModelState.IsValid)
                {
                    vehiclePurchase.SaleDate = newVehicleSales.SaleDate;
                    vehiclePurchase.SaleAmount = newVehicleSales.SaleAmount;
                    vehiclePurchase.SaleAmountCurrency = newVehicleSales.SaleAmountCurrency;

                    newVehicleSale.Description = newVehicleSales.Description;
                    newVehicleSale.PurchaseDate = newVehicleSales.PurchaseDate;
                    newVehicleSale.SaleAmount = newVehicleSales.SaleAmount;
                    newVehicleSale.SaleAmountCurrency = newVehicleSales.SaleAmountCurrency;
                    newVehicleSale.SaleDate = newVehicleSales.SaleDate;
                    newVehicleSale.SalesmanBonus = newVehicleSales.SalesmanBonus;
                    newVehicleSale.SalesmanBonusCurrency = newVehicleSales.SalesmanBonusCurrency;
                    newVehicleSale.PurchasedSalesmanId = newVehicleSales.PurchasedSalesmanId;
                    newVehicleSale.LicencePlate = newVehicleSales.LicencePlate;
                    newVehicleSale.SalesmanId = newVehicleSales.SalesmanId;
                    newVehicleSale.VehicleCost = newVehicleSales.VehicleCost;
                    newVehicleSale.VehicleCostCurrency = newVehicleSales.VehicleCostCurrency;
                    newVehicleSale.VehiclePurchaseId = newVehicleSales.VehiclePurchaseId;
                    newVehicleSale.WarrantyPlus = newVehicleSales.WarrantyPlus;

                    _context.Update(vehiclePurchase);
                    _context.Update(newVehicleSale);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Sıfır Araç Satışı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Sıfır Araç Satışı Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var newVehicleSales = await _context.NewVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.Salesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .FirstOrDefaultAsync(m => m.Id == id);

            newVehicleSales = GetAllEnumNamesHelper.GetEnumName(newVehicleSales);

            if (newVehicleSales == null)
            {
                return View("Error");
            }

            return View(newVehicleSales);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newVehicleSales = await _context.NewVehicleSales.FindAsync(id);

            if (newVehicleSales != null)
            {
                var vehiclePurchase = await _context.VehiclePurchases.FirstOrDefaultAsync(vp => vp.Id == newVehicleSales.VehiclePurchaseId);
                if (vehiclePurchase != null)
                {
                    vehiclePurchase.IsSold = false;
                    vehiclePurchase.SaleDate = null;
                    vehiclePurchase.SaleAmount = null;
                    vehiclePurchase.SaleAmountCurrency = 0;

                    _context.VehiclePurchases.Update(vehiclePurchase);
                }

                _context.NewVehicleSales.Remove(newVehicleSales);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Sıfır Araç Satışı Silinmiştir!" });
            }
            return BadRequest("Sıfır Araç Satışı Silinirken Bir Hata Oluştu!");
        }
    }
}
