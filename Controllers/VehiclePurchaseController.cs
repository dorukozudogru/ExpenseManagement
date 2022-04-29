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
using static ExpenseManagement.Models.ViewModels.ReportViewModel;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe, Plaza"))]
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
        public async Task<IActionResult> Post(bool isFiltered, DateTime startDate, DateTime finishDate, double from, double to, int monthId, string model, string chassis)
        {
            var requestFormData = Request.Form;

            var vehiclePurchaseContext = await _context.VehiclePurchases
                .Include(c => c.CarModel)
                .Include(cb => cb.CarModel.CarBrand)
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (startDate != DateTime.MinValue && finishDate != DateTime.MinValue)
                {
                    vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.PurchaseDate >= startDate && e.PurchaseDate <= finishDate).ToList();
                }
                if (from != 0 && to != 0)
                {
                    vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.PurchaseAmount >= from && e.PurchaseAmount <= to).ToList();
                }
                if (monthId != 0)
                {
                    vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.PurchaseDate != null && e.PurchaseDate.Month == monthId).ToList();
                }
                if (model != null)
                {
                    var modelId = await _context.CarModels.FirstOrDefaultAsync(c => c.Name == model);
                    vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.CarModelId == modelId.Id).ToList();
                }
                if (chassis != null)
                {
                    vehiclePurchaseContext = vehiclePurchaseContext.Where(e => e.Chassis.ToUpper().Contains(chassis.ToUpper())).ToList();
                }
            }

            vehiclePurchaseContext = GetAllEnumNamesHelper.GetEnumName(vehiclePurchaseContext);

            List<VehiclePurchases> listItems = ProcessCollection(vehiclePurchaseContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(vehiclePurchaseContext);

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
            ViewData["RegistrationFeeId"] = new SelectList(_context.RegistrationFees.OrderBy(s => s.RegistrationFee), "Id", "RegistrationFee");
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
            ViewBag.RegistrationFeeId = new SelectList(_context.RegistrationFees.OrderBy(s => s.RegistrationFee), "Id", "RegistrationFee");
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
                .Include(r => r.RegistrationFee)
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
                .Include(r => r.RegistrationFee)
                .FirstOrDefaultAsync(m => m.Id == id);

            var newVehicleSale = await _context.NewVehicleSales.FirstOrDefaultAsync(nvs => nvs.VehiclePurchaseId == id);

            if (vehiclePurchase != null)
            {
                if (ModelState.IsValid)
                {
                    if (newVehicleSale != null)
                    {
                        newVehicleSale.PurchaseDate = vehiclePurchases.PurchaseDate;
                        newVehicleSale.VehicleCost = vehiclePurchases.IncludingRegistrationFee;
                        _context.Update(newVehicleSale);
                    }

                    vehiclePurchase.CarModelId = _context.CarModels.FirstOrDefault(x => x.Name == vehiclePurchases.CarModel.Name).Id;
                    vehiclePurchase.IsNew = vehiclePurchases.IsNew;
                    vehiclePurchase.IsSold = vehiclePurchases.IsSold;
                    vehiclePurchase.Chassis = vehiclePurchases.Chassis;
                    vehiclePurchase.PurchaseDate = vehiclePurchases.PurchaseDate;
                    vehiclePurchase.SaleDate = vehiclePurchases.SaleDate;
                    vehiclePurchase.ValorDate = vehiclePurchases.ValorDate;
                    vehiclePurchase.PaymentDate = vehiclePurchases.PaymentDate;
                    vehiclePurchase.PurchaseAmount = vehiclePurchases.PurchaseAmount;
                    vehiclePurchase.PurchaseAmountCurrency = vehiclePurchases.PurchaseAmountCurrency;
                    vehiclePurchase.AmountToBePaid = vehiclePurchases.AmountToBePaid;

                    vehiclePurchase.OTVPercent = vehiclePurchases.OTVPercent;
                    vehiclePurchase.OTV = vehiclePurchases.OTV;
                    vehiclePurchase.KDV = vehiclePurchases.KDV;
                    vehiclePurchase.RegistrationFeeId = vehiclePurchases.RegistrationFeeId;
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
                .Where(vp => vp.PaymentDate != null && vp.PaymentDate >= DateTime.Now)
                .OrderBy(v => v.PurchaseDate)
                .AsNoTracking()
                .ToListAsync();

            List<VehiclePurchases> listItems = ProcessCollectionT(vehiclePurchaseContext, requestFormData);

            foreach (var item in listItems)
            {
                if (item.ValorDate != null)
                {
                    item.ValorEndDate = item.PurchaseDate.AddDays((double)item.ValorDate);
                }
            }

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(vehiclePurchaseContext.OrderBy(v => v.PurchaseDate));

            var response = new PaginatedResponse<VehiclePurchases>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = vehiclePurchaseContext.Count,
                RecordsTotal = vehiclePurchaseContext.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> MonthlyTotalPost()
        {
            var requestFormData = Request.Form;

            var vehiclePurchase = await _context.VehiclePurchases
                .GroupBy(i => new
                {
                    i.PurchaseDate.Year,
                    i.PurchaseDate.Month
                })
                .Select(i => new GeneralResponse
                {
                    Year = i.Key.Year,
                    Month = i.Key.Month,
                    Count = i.Count()
                })
                .ToListAsync();

            vehiclePurchase = vehiclePurchase.OrderByDescending(i => i.Year).ThenByDescending(i => i.Month).ToList();

            vehiclePurchase = GetAllEnumNamesHelper.GetEnumName(vehiclePurchase);

            List<GeneralResponse> listItems = ProcessCollection(vehiclePurchase, requestFormData);

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = vehiclePurchase.Count,
                RecordsTotal = vehiclePurchase.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> YearlyTotalPost()
        {
            var requestFormData = Request.Form;

            var vehiclePurchase = await _context.VehiclePurchases
                .GroupBy(i => new
                {
                    i.PurchaseDate.Year
                })
                .Select(i => new GeneralResponse
                {
                    Year = i.Key.Year,
                    Count = i.Count()
                })
                .ToListAsync();

            vehiclePurchase = vehiclePurchase.OrderByDescending(i => i.Year).ToList();


            List<GeneralResponse> listItems = ProcessCollection(vehiclePurchase, requestFormData);

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = vehiclePurchase.Count,
                RecordsTotal = vehiclePurchase.Count
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public ActionResult ExportPurchases()
        {
            var stream = ExportAllSales(JsonConvert.DeserializeObject<List<VehiclePurchases>>(FakeSession.Instance.Obj), "Araç Alış Listesi");
            string fileName = String.Format("{0}.xlsx", "Araç Alış Listesi");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public MemoryStream ExportAllSales(List<VehiclePurchases> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            items = GetAllEnumNamesHelper.GetEnumName(items);

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("Araç Alış Listesi");

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
                ws.Cells[1, 4].Value = "Araç Sıfır Mı?";
                ws.Cells[1, 5].Value = "Araç Satıldı Mı?";
                ws.Cells[1, 6].Value = "Alış Tarihi";
                ws.Cells[1, 7].Value = "Satış Tarihi";
                ws.Cells[1, 8].Value = "Valör";
                ws.Cells[1, 9].Value = "Valör Bitiş Tarihi";
                ws.Cells[1, 10].Value = "Ödeme Tarihi";
                ws.Cells[1, 11].Value = "Şase No";
                ws.Cells[1, 12].Value = "Araç Alış Fiyatı";
                ws.Cells[1, 13].Value = "Trafik Tescil Bedeli Dahil Fiyatı";
                ws.Cells[1, 14].Value = "Araç Satış Fiyatı";

                ws.Column(6).Style.Numberformat.Format = "dd-mmmm-yyyy";
                ws.Column(7).Style.Numberformat.Format = "dd-mmmm-yyyy";
                ws.Column(9).Style.Numberformat.Format = "dd-mmmm-yyyy";
                ws.Column(10).Style.Numberformat.Format = "dd-mmmm-yyyy";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Id;
                    ws.Cells[c, 2].Value = items[c - 2].CarModel.CarBrand.Name;
                    ws.Cells[c, 3].Value = items[c - 2].CarModel.Name;
                    ws.Cells[c, 4].Value = items[c - 2].IsNew;
                    ws.Cells[c, 5].Value = items[c - 2].IsSold;
                    ws.Cells[c, 6].Value = items[c - 2].PurchaseDate;
                    ws.Cells[c, 7].Value = items[c - 2].SaleDate;
                    ws.Cells[c, 8].Value = items[c - 2].ValorDate;

                    if (items[c - 2].ValorDate != null)
                    {
                        items[c - 2].ValorEndDate = items[c - 2].PurchaseDate.AddDays((double)items[c - 2].ValorDate);
                        ws.Cells[c, 9].Value = items[c - 2].ValorEndDate;
                    }
                    else
                    {
                        ws.Cells[c, 9].Value = "";
                    }

                    if (items[c - 2].PaymentDate != null)
                    {
                        ws.Cells[c, 10].Value = items[c - 2].PaymentDate;
                    }
                    else
                    {
                        ws.Cells[c, 10].Value = "";
                    }

                    ws.Cells[c, 11].Value = items[c - 2].Chassis;
                    ws.Cells[c, 12].Value = items[c - 2].PurchaseAmount + " " + items[c - 2].PurchaseAmountCurrencyName;

                    if (items[c - 2].IncludingRegistrationFee != null)
                    {
                        ws.Cells[c, 13].Value = items[c - 2].IncludingRegistrationFee + " ₺";
                    }
                    else
                    {
                        ws.Cells[c, 13].Value = "";
                    }

                    if (items[c - 2].SaleAmount != null)
                    {
                        ws.Cells[c, 14].Value = items[c - 2].SaleAmount + " " + items[c - 2].SaleAmountCurrencyName;
                    }
                    else
                    {
                        ws.Cells[c, 14].Value = "";
                    }

                    ws.Column(12).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].PurchaseAmountCurrencyName);
                    ws.Column(13).Style.Numberformat.Format = String.Format("#,##0.00 {0}", "₺");
                    ws.Column(14).Style.Numberformat.Format = String.Format("#,##0.00 {0}", items[c - 2].SaleAmountCurrencyName);
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells["A1:N" + items.Count + 2].AutoFilter = true;

                ws.Column(14).PageBreak = true;
                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                ws.PrinterSettings.Scale = 100;

                p.Save();
            }
            AddExportAudit(pageName, HttpContext?.User?.Identity?.Name, _context);
            return stream;
        }

        [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
        public ActionResult ExportVehicles()
        {
            var stream = ExportSaleReport(JsonConvert.DeserializeObject<List<VehiclePurchases>>(FakeSession.Instance.Obj), "Ödeme Tarihi Gelen Araçlar");
            string fileName = String.Format("{0}.xlsx", "Ödeme Tarihi Gelen Araçlar");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        public MemoryStream ExportSaleReport(List<VehiclePurchases> items, string pageName)
        {
            var purchasedDates = items
                .GroupBy(i => i.PurchaseDate)
                .OrderBy(i => i.Key);

            var stream = new System.IO.MemoryStream();

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add(pageName);

                using (var range = ws.Cells[1, 1, 1, 8])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "No";
                ws.Cells[1, 2].Value = "Araç Cinsi";
                ws.Cells[1, 3].Value = "Şase No";
                ws.Cells[1, 4].Value = "Alım Tarihi";
                ws.Cells[1, 5].Value = "Ödeme Tarihi";
                ws.Cells[1, 6].Value = "Valör";
                ws.Cells[1, 7].Value = "Kalan Gün";
                ws.Cells[1, 8].Value = "Tutar";

                ws.Row(1).Style.Font.Bold = true;

                ws.Column(4).Style.Numberformat.Format = "dd-mmmm-yyyy";
                ws.Column(5).Style.Numberformat.Format = "dd-mmmm-yyyy";
                ws.Column(8).Style.Numberformat.Format = String.Format("#,##0.00 ₺");

                int row = 2;
                int count = 1;
                double sum = 0.0;
                double totalSum = 0.0;

                foreach (var item in purchasedDates)
                {
                    var lastItems = items.Where(i => i.PurchaseDate == item.Key).ToList();

                    foreach (var lastItem in lastItems)
                    {
                        var rd = 0;
                        if (lastItem.PaymentDate != DateTime.MinValue)
                        {
                            rd = (lastItem.PaymentDate.Value.Date - DateTime.Now.Date).Days;
                        }
                        
                        ws.Cells[row, 1].Value = count;
                        ws.Cells[row, 2].Value = lastItem.CarModel.Name;
                        ws.Cells[row, 3].Value = lastItem.Chassis;
                        ws.Cells[row, 4].Value = lastItem.PurchaseDate;
                        ws.Cells[row, 5].Value = lastItem.PaymentDate;
                        ws.Cells[row, 6].Value = lastItem.ValorDate;
                        ws.Cells[row, 7].Value = rd;
                        ws.Cells[row, 8].Value = lastItem.AmountToBePaid;

                        ws.Cells[row, 1, row + 1, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[row, 1, row + 1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[row, 1, row + 1, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[row, 1, row + 1, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        row++;
                        count++;
                        sum += lastItem.AmountToBePaid;
                        totalSum += lastItem.AmountToBePaid;
                    }

                    using (var range = ws.Cells[row, 1, row, 8])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    ws.Cells[row, 8].Value = sum;
                    ws.Cells[row, 1, row, 7].Merge = true;
                    ws.Cells[row, 1, row, 7].Value = "TOPLAM";

                    row += 2;

                    ws.Cells[row, 1].Value = "";
                    ws.Cells[row, 2].Value = "";
                    ws.Cells[row, 3].Value = "";
                    ws.Cells[row, 4].Value = "";
                    ws.Cells[row, 5].Value = "";
                    ws.Cells[row, 6].Value = "";
                    ws.Cells[row, 7].Value = "";
                    ws.Cells[row, 8].Value = "";
                    row++;
                    count = 1;
                    sum = 0.0;
                }

                var lastRow = ws.Dimension.End.Row;
                var lastColumn = ws.Dimension.End.Column;

                using (var range = ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Green);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[lastRow + 1, 8].Value = totalSum;

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn - 1].Merge = true;
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn - 1].Value = "GENEL TOPLAM";

                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                ws.Cells[1, 1, lastRow + 1, lastColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[1, 1, lastRow + 1, lastColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Column(8).Width = 20;
                ws.Column(8).PageBreak = true;

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
