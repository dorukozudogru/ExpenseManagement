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
using static ExpenseManagement.Helpers.AddExportAuditHelper;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

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

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public ActionResult ExportSales()
        {
            var stream = ExportAllSales(_context.NewVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.Salesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .AsNoTracking()
                .ToList(), "Sıfır Araç Satışı");
            string fileName = String.Format("{0}.xlsx", "Sıfır Araç Satışı Listesi");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public MemoryStream ExportAllSales(List<NewVehicleSales> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            items = GetAllEnumNamesHelper.GetEnumName(items);

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("Sıfır Araç Satışı");

                using (var range = ws.Cells[1, 1, 1, 13])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "ID";
                ws.Cells[1, 2].Value = "Marka";
                ws.Cells[1, 3].Value = "Model";
                ws.Cells[1, 4].Value = "Şase No";
                ws.Cells[1, 5].Value = "Plaka";
                ws.Cells[1, 6].Value = "Alan Danışman";
                ws.Cells[1, 7].Value = "Satan Danışman";
                ws.Cells[1, 8].Value = "Araç Satış Fiyatı";
                ws.Cells[1, 9].Value = "Alış Tarihi";
                ws.Cells[1, 10].Value = "Satış Tarihi";
                ws.Cells[1, 11].Value = "Araç Maliyeti";
                ws.Cells[1, 12].Value = "Artı Garanti";
                ws.Cells[1, 13].Value = "Açıklama";

                ws.Column(9).Style.Numberformat.Format = "dd-mmmm-yyyy";
                ws.Column(10).Style.Numberformat.Format = "dd-mmmm-yyyy";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Id;
                    ws.Cells[c, 2].Value = items[c - 2].VehiclePurchase.CarModel.CarBrand.Name;
                    ws.Cells[c, 3].Value = items[c - 2].VehiclePurchase.CarModel.Name;
                    ws.Cells[c, 4].Value = items[c - 2].VehiclePurchase.Chassis;
                    ws.Cells[c, 5].Value = items[c - 2].LicencePlate;
                    if (items[c - 2].PurchasedSalesman != null)
                    {
                        ws.Cells[c, 6].Value = items[c - 2].PurchasedSalesman.Name;
                    }
                    if (items[c - 2].Salesman != null)
                    {
                        ws.Cells[c, 7].Value = items[c - 2].Salesman.Name;
                    }
                    ws.Cells[c, 8].Value = items[c - 2].SaleAmount + " " + items[c - 2].SaleAmountCurrencyName;
                    ws.Cells[c, 9].Value = items[c - 2].PurchaseDate;
                    ws.Cells[c, 10].Value = items[c - 2].SaleDate;
                    ws.Cells[c, 11].Value = items[c - 2].VehicleCost + " " + items[c - 2].VehicleCostCurrencyName;
                    ws.Cells[c, 12].Value = items[c - 2].WarrantyPlus;
                    ws.Cells[c, 13].Value = items[c - 2].Description;

                    ws.Column(8).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].SaleAmountCurrencyName);
                    ws.Column(11).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].VehicleCostCurrencyName);
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells["A1:M" + items.Count + 2].AutoFilter = true;

                ws.Column(13).PageBreak = true;
                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                ws.PrinterSettings.Scale = 100;

                p.Save();
            }
            AddExportAudit(pageName, HttpContext?.User?.Identity?.Name, _context);
            return stream;
        }
    }
}
