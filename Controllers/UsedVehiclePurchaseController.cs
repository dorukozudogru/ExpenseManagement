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
using static ExpenseManagement.Helpers.AddExportAuditHelper;
using ExpenseManagement.Helpers;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using ExpenseManagement.Models.ViewModels;
using Newtonsoft.Json;

namespace ExpenseManagement.Controllers
{
    [Authorize]
    public class UsedVehiclePurchaseController : Controller
    {
        private readonly ExpenseContext _context;

        public UsedVehiclePurchaseController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, DateTime startDate, DateTime finishDate, double from, double to, int monthId, string model, string chassis)
        {
            var requestFormData = Request.Form;

            var uvpContext = await _context.UsedVehiclePurchases
                .Include(c => c.CarModel)
                .Include(c => c.CarModel.CarBrand)
                .Include(c => c.PurchasedSalesman)
                .Include(c => c.SoldSalesman)
                .AsNoTracking()
                .ToListAsync();

            //if (isFiltered != false)
            //{
            //    if (startDate != DateTime.MinValue && finishDate != DateTime.MinValue)
            //    {
            //        vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.PurchaseDate >= startDate && e.PurchaseDate <= finishDate).ToList();
            //    }
            //    if (from != 0 && to != 0)
            //    {
            //        vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.PurchaseAmount >= from && e.PurchaseAmount <= to).ToList();
            //    }
            //    if (monthId != 0)
            //    {
            //        vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.PurchaseDate != null && e.PurchaseDate.Month == monthId).ToList();
            //    }
            //    if (model != null)
            //    {
            //        var modelId = await _context.CarModels.FirstOrDefaultAsync(c => c.Name == model);
            //        vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.CarModelId == modelId.Id).ToList();
            //    }
            //    if (chassis != null)
            //    {
            //        vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.Chassis.ToUpper().Contains(chassis.ToUpper())).ToList();
            //    }
            //}

            List<UsedVehiclePurchases> listItems = ProcessCollection(uvpContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(uvpContext);

            var response = new PaginatedResponse<UsedVehiclePurchases>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = uvpContext.Count,
                RecordsTotal = uvpContext.Count
            };

            return Ok(response);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var uvpContext = await _context.UsedVehiclePurchases
                .Include(c => c.CarModel)
                .Include(c => c.CarModel.CarBrand)
                .Include(c => c.PurchasedSalesman)
                .Include(c => c.SoldSalesman)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (uvpContext == null)
            {
                return View("Error");
            }

            return View(uvpContext);
        }

        public IActionResult Create()
        {
            ViewData["CarModelId"] = new SelectList(_context.CarModels, "Id", "Name");
            ViewData["PurchasedSalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");
            ViewData["SoldSalesmanId"] = new SelectList(_context.Salesmans, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UsedVehiclePurchases usedVehiclePurchases)
        {
            if (ModelState.IsValid)
            {
                usedVehiclePurchases.CarModelId = _context.CarModels.FirstOrDefault(x => x.Name == usedVehiclePurchases.CarModel.Name).Id;

                _context.Add(usedVehiclePurchases);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "2. El Araç Alımı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("2. El Araç Alımı Oluşturulurken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewBag.CarBrands = new SelectList(_context.CarBrands.OrderBy(x => x.Name), "Id", "Name");
            ViewBag.CarModels = new SelectList(_context.CarModels.OrderBy(x => x.Name), "Name", "Name");
            ViewBag.PurchasedSalesman = new SelectList(_context.Salesmans, "Id", "Name");
            ViewBag.SoldSalesman = new SelectList(_context.Salesmans, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var usedVehiclePurchases = await _context.UsedVehiclePurchases
                .Include(c => c.CarModel)
                .Include(c => c.CarModel.CarBrand)
                .Include(c => c.PurchasedSalesman)
                .Include(c => c.SoldSalesman)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usedVehiclePurchases == null)
            {
                return View("Error");
            }

            return Ok(usedVehiclePurchases);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UsedVehiclePurchases usedVehiclePurchases)
        {
            var usedVehiclePurchase = await _context.UsedVehiclePurchases
                .Include(c => c.CarModel)
                .Include(c => c.CarModel.CarBrand)
                .Include(c => c.PurchasedSalesman)
                .Include(c => c.SoldSalesman)
                .FirstOrDefaultAsync(m => m.Id == id);

            var usedVehicleSale = await _context.UsedVehicleSales.FirstOrDefaultAsync(nvs => nvs.VehiclePurchaseId == id);

            if (usedVehiclePurchase != null)
            {
                if (ModelState.IsValid)
                {
                    if (usedVehicleSale != null)
                    {
                        usedVehicleSale.PurchaseDate = usedVehiclePurchases.PurchaseDate;
                        usedVehicleSale.PurchaseAmount = usedVehiclePurchases.PurchaseAmount;
                        usedVehicleSale.PurchasedSalesmanId = usedVehiclePurchases.PurchasedSalesmanId;

                        _context.Update(usedVehicleSale);
                    }

                    usedVehiclePurchase.Buyer = usedVehiclePurchases.Buyer;
                    usedVehiclePurchase.CarModelId = _context.CarModels.FirstOrDefault(x => x.Name == usedVehiclePurchases.CarModel.Name).Id;
                    usedVehiclePurchase.IsSold = usedVehiclePurchases.IsSold;
                    usedVehiclePurchase.KM = usedVehiclePurchases.KM;
                    usedVehiclePurchase.LicencePlate = usedVehiclePurchases.LicencePlate;
                    usedVehiclePurchase.Profit = usedVehiclePurchases.Profit;
                    usedVehiclePurchase.PurchaseAmount = usedVehiclePurchases.PurchaseAmount;
                    usedVehiclePurchase.PurchaseDate = usedVehiclePurchases.PurchaseDate;
                    usedVehiclePurchase.PurchasedSalesmanBonus = usedVehiclePurchases.PurchasedSalesmanBonus;
                    usedVehiclePurchase.PurchasedSalesmanId = usedVehiclePurchases.PurchasedSalesmanId;
                    usedVehiclePurchase.SaleAmount = usedVehiclePurchases.SaleAmount;
                    usedVehiclePurchase.SaleDate = usedVehiclePurchases.SaleDate;
                    usedVehiclePurchase.Seller = usedVehiclePurchases.Seller;
                    usedVehiclePurchase.SoldSalesmanBonus = usedVehiclePurchases.SoldSalesmanBonus;
                    usedVehiclePurchase.SoldSalesmansId = usedVehiclePurchases.SoldSalesmansId;

                    _context.Update(usedVehiclePurchase);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "2. El Araç Alımı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("2. El Araç Alımı Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var usedVehiclePurchase = await _context.UsedVehiclePurchases
                .Include(c => c.CarModel)
                .Include(c => c.CarModel.CarBrand)
                .Include(c => c.PurchasedSalesman)
                .Include(c => c.SoldSalesman)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usedVehiclePurchase == null)
            {
                return View("Error");
            }

            return View(usedVehiclePurchase);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hasAnyUsedVehicleSales = await _context.UsedVehicleSales
                .FirstOrDefaultAsync(m => m.VehiclePurchaseId == id);

            if (hasAnyUsedVehicleSales == null)
            {
                var usedVehiclePurchases = await _context.UsedVehiclePurchases.FindAsync(id);
                _context.UsedVehiclePurchases.Remove(usedVehiclePurchases);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "2. El Araç Alımı Silinmiştir!" });
            }
            return BadRequest("Araç Satışı Bulunduğundan Dolayı Silinemez!");
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public ActionResult ExportPurchases()
        {
            var stream = ExportAllSales(JsonConvert.DeserializeObject<List<UsedVehiclePurchases>>(FakeSession.Instance.Obj), "2. El Araç Alış Listesi");
            string fileName = String.Format("{0}.xlsx", "2. El Araç Alış Listesi");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public MemoryStream ExportAllSales(List<UsedVehiclePurchases> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("2. El Araç Alış Listesi");

                using (var range = ws.Cells[1, 1, 1, 16])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "ID";
                ws.Cells[1, 2].Value = "Marka";
                ws.Cells[1, 3].Value = "Model";
                ws.Cells[1, 4].Value = "KM";
                ws.Cells[1, 5].Value = "Plaka";
                ws.Cells[1, 6].Value = "Aracın Alındığı Kişi";
                ws.Cells[1, 7].Value = "Alış Tarihi";
                ws.Cells[1, 8].Value = "Alan Danışman";
                ws.Cells[1, 9].Value = "Alış Fiyatı";
                ws.Cells[1, 10].Value = "Alan Danışman Primi";
                ws.Cells[1, 11].Value = "Aracın Satıldığı Kişi";
                ws.Cells[1, 12].Value = "Satış Tarihi";
                ws.Cells[1, 13].Value = "Satan Danışman";
                ws.Cells[1, 14].Value = "Satış Fiyatı";
                ws.Cells[1, 15].Value = "Satan Danışman Primi";
                ws.Cells[1, 16].Value = "Kâr";

                ws.Column(7).Style.Numberformat.Format = "dd-mmmm-yyyy";
                ws.Column(12).Style.Numberformat.Format = "dd-mmmm-yyyy";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Id;
                    ws.Cells[c, 2].Value = items[c - 2].CarModel.CarBrand.Name;
                    ws.Cells[c, 3].Value = items[c - 2].CarModel.Name;
                    ws.Cells[c, 4].Value = items[c - 2].KM;
                    ws.Cells[c, 5].Value = items[c - 2].LicencePlate;
                    ws.Cells[c, 6].Value = items[c - 2].Seller;
                    ws.Cells[c, 7].Value = items[c - 2].PurchaseDate;
                    ws.Cells[c, 8].Value = items[c - 2].PurchasedSalesman.Name;
                    ws.Cells[c, 9].Value = items[c - 2].PurchaseAmount;
                    ws.Cells[c, 10].Value = items[c - 2].PurchasedSalesmanBonus;
                    ws.Cells[c, 11].Value = items[c - 2].Buyer;
                    ws.Cells[c, 12].Value = items[c - 2].SaleDate;
                    ws.Cells[c, 13].Value = items[c - 2].SoldSalesman.Name;
                    ws.Cells[c, 14].Value = items[c - 2].SaleAmount;
                    ws.Cells[c, 15].Value = items[c - 2].SoldSalesmanBonus;
                    ws.Cells[c, 16].Value = items[c - 2].Profit;

                    ws.Column(9).Style.Numberformat.Format = String.Format("#,##0.00 {0}", "₺");
                    ws.Column(14).Style.Numberformat.Format = String.Format("#,##0.00 {0}", "₺");
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells["A1:P" + items.Count + 2].AutoFilter = true;

                ws.Column(16).PageBreak = true;
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
