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

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public ActionResult ExportSales()
        {
            var stream = ExportAllSales(_context.UsedVehicleSales
                .Include(n => n.PurchasedSalesman)
                .Include(n => n.SoldSalesman)
                .Include(n => n.VehiclePurchase)
                .Include(n => n.VehiclePurchase.CarModel)
                .Include(n => n.VehiclePurchase.CarModel.CarBrand)
                .AsNoTracking()
                .ToList(), "2. El Araç Satışı");
            string fileName = String.Format("{0}.xlsx", "2. El Araç Satışı Listesi");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public MemoryStream ExportAllSales(List<UsedVehicleSales> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            items = GetAllEnumNamesHelper.GetEnumName(items);

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("2. El Araç Satışı");

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
                ws.Cells[1, 6].Value = "KM";
                ws.Cells[1, 7].Value = "Alış Tarihi";
                ws.Cells[1, 8].Value = "Satış Tarihi";
                ws.Cells[1, 9].Value = "Alan Danışman";
                ws.Cells[1, 10].Value = "Satan Danışman";
                ws.Cells[1, 11].Value = "Araç Alış Fiyatı";
                ws.Cells[1, 12].Value = "Araç Satış Fiyatı";
                ws.Cells[1, 13].Value = "Açıklama";

                ws.Column(7).Style.Numberformat.Format = "dd-mmmm-yyyy";
                ws.Column(8).Style.Numberformat.Format = "dd-mmmm-yyyy";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Id;
                    ws.Cells[c, 2].Value = items[c - 2].VehiclePurchase.CarModel.CarBrand.Name;
                    ws.Cells[c, 3].Value = items[c - 2].VehiclePurchase.CarModel.Name;
                    ws.Cells[c, 4].Value = items[c - 2].VehiclePurchase.Chassis;
                    ws.Cells[c, 5].Value = items[c - 2].LicencePlate;
                    ws.Cells[c, 6].Value = items[c - 2].KM;
                    ws.Cells[c, 7].Value = items[c - 2].PurchaseDate;
                    ws.Cells[c, 8].Value = items[c - 2].SaleDate;
                    ws.Cells[c, 9].Value = items[c - 2].PurchasedSalesman.Name;
                    ws.Cells[c, 10].Value = items[c - 2].SoldSalesman.Name;
                    ws.Cells[c, 11].Value = items[c - 2].PurchaseAmount + " " + items[c - 2].PurchaseAmountCurrencyName;
                    ws.Cells[c, 12].Value = items[c - 2].SaleAmount + " " + items[c - 2].SaleAmountCurrencyName;
                    ws.Cells[c, 13].Value = items[c - 2].Description;

                    ws.Column(11).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].PurchaseAmountCurrencyName);
                    ws.Column(12).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].SaleAmountCurrencyName);
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
