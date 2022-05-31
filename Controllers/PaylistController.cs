using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using static ExpenseManagement.Models.Expenses;
using System.Security.Claims;
using ExpenseManagement.Helpers;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using static ExpenseManagement.Helpers.AddExportAuditHelper;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using static ExpenseManagement.Models.ViewModels.ReportViewModel;
using Newtonsoft.Json;
using ExpenseManagement.Models.ViewModels;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
    public class PaylistController : Controller
    {
        private readonly ExpenseContext _context;

        public PaylistController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool state, bool isFiltered, DateTime startDate, DateTime finishDate)
        {
            var paylistContext = await _context.Paylists
                .Where(e => e.State == state)
                .OrderBy(e => e.Date)
                .ThenByDescending(e => e.Amount)
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (startDate != DateTime.MinValue && finishDate != DateTime.MinValue)
                {
                    paylistContext = paylistContext.Where(p => p.Date >= startDate && p.Date <= finishDate).ToList();
                }
                FakeSession.Instance.Obj = JsonConvert.SerializeObject(paylistContext);
            }

            return Ok(paylistContext);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Date,Amount,PersonToBePaid")] Paylists paylists)
        {
            if (ModelState.IsValid)
            {
                paylists.State = false;

                _context.Add(paylists);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Ödeme Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Ödeme Kaydedilirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var paylist = await _context.Paylists.FindAsync(id);

            if (paylist == null)
            {
                return View("Error");
            }
            return View(paylist);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Paylists paylists)
        {
            var paylist = await _context.Paylists.FindAsync(id);

            if (paylist != null)
            {
                if (ModelState.IsValid)
                {
                    paylist.Amount = paylists.Amount;
                    paylist.Date = paylists.Date;
                    paylist.PersonToBePaid = paylists.PersonToBePaid;

                    _context.Update(paylist);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Ödeme Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Ödeme Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditState(int? id)
        {
            var paylist = await _context.Paylists.FindAsync(id);
            if (paylist != null)
            {
                if (paylist.State == true)
                {
                    paylist.State = false;
                }
                else if (paylist.State == false)
                {
                    paylist.State = true;
                }

                _context.Update(paylist);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Kayıt Başarıyla Güncellendi!" });
            }
            return BadRequest("Ödeme Güncellenirken Bir Hata Oluştu!");
        }

        public IActionResult GetPerson()
        {
            var name = HttpContext.Request.Query["term"].ToString();
            var person = _context.Paylists.Where(c => c.PersonToBePaid.Contains(name)).GroupBy(c => c.PersonToBePaid).Select(c => c.Key).ToList();
            List<string> data = new List<string>();
            if (person.Count != 0)
            {
                for (int i = 0; i < person.Count; i++)
                {
                    if (i < 5)
                    {
                        data.Add(person[i]);
                    }
                }
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> DailyTotalPost()
        {
            var requestFormData = Request.Form;

            var paylistContext = await _context.Paylists
                .Where(e => e.State == false)
                .OrderBy(e => e.Date.Day)
                .AsNoTracking()
                .ToListAsync();

            var groupedPaylist = paylistContext
                .GroupBy(e => new
                {
                    e.Date
                })
                .Select(e => new PaylistResponse
                {
                    Date = e.Key.Date,
                    TotalAmount = e.Sum(x => x.Amount),
                })
                .OrderBy(e => e.Date)
                .ToList();

            List<PaylistResponse> listItems = ProcessCollection(groupedPaylist, requestFormData);

            var response = new PaginatedResponse<PaylistResponse>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = groupedPaylist.Count,
                RecordsTotal = groupedPaylist.Count
            };

            return Ok(response);
        }

        public ActionResult ExportAllActivePaylist()
        {
            var stream = ExportPaylist(_context.Paylists
                .Where(s => s.State == false).ToList(), "Ödeme Listesi");
            string fileName = String.Format("{0}.xlsx", "Ödeme Listesi");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        public ActionResult ExportPassivePaylist()
        {
            var stream = ExportPaylist(JsonConvert.DeserializeObject<List<Paylists>>(FakeSession.Instance.Obj), "Ödeme Listesi");
            string fileName = String.Format("{0}.xlsx", "Ödeme Listesi");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        public MemoryStream ExportPaylist(List<Paylists> items, string pageName)
        {
            var dates = items
                .GroupBy(i => i.Date)
                .OrderBy(i => i.Key);

            string[] days = { "", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar" };

            var stream = new System.IO.MemoryStream();

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("Ödemeler");

                ws.Cells[1, 1, 1, 3].Merge = true;
                ws.Cells[1, 1, 1, 3].Style.Font.Bold = true;
                ws.Cells[1, 1, 1, 3].Value = dates.First().Key.ToString("dd.MM.yyyy") + " - " + dates.Last().Key.ToString("dd.MM.yyyy") + " TARİHLERİ ARASI ÖDEME PLANI";

                using (var range = ws.Cells[2, 1, 2, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);

                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                ws.Cells[2, 1].Value = "Tarih";
                ws.Cells[2, 2].Value = "Tutar";
                ws.Cells[2, 3].Value = "Ödeme Yapılacak Kişi/Kurum";

                ws.Column(2).Style.Numberformat.Format = String.Format("#,##0.00 ₺");

                ws.Row(2).Style.Font.Bold = true;

                int tr = 0;
                int row = 3;
                int column = 1;
                int count = 1;
                double sum = 0.0;
                double totalSum = 0.0;

                foreach (var item in dates)
                {
                    var lastItems = items.Where(i => i.Date == item.Key).ToList();
                    tr = 0;

                    foreach (var lastItem in lastItems)
                    {
                        if (tr != 1)
                        {
                            ws.Cells[row, column, row + lastItems.Count - 1, column].Merge = true;
                            ws.Cells[row, column, row + lastItems.Count - 1, column].Value = days[(int)item.Key.DayOfWeek] + Environment.NewLine + item.Key.Date.ToString("dd.MM.yyyy");

                            tr = 1;
                        }

                        ws.Cells[row, 2].Value = lastItem.Amount;
                        ws.Cells[row, 3].Value = lastItem.PersonToBePaid;

                        ws.Row(row).Height = 30;

                        if (count < lastItems.Count)
                        {
                            ws.Cells[row, 1, row + 1, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws.Cells[row, 1, row + 1, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[row, 1, row + 1, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws.Cells[row, 1, row + 1, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        else if (count == 1)
                        {
                            ws.Cells[row, 1, row, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws.Cells[row, 1, row, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[row, 1, row, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws.Cells[row, 1, row, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        row++;
                        count++;
                        sum += lastItem.Amount;
                        totalSum += lastItem.Amount;
                    }

                    using (var range = ws.Cells[row, 1, row, 3])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                        range.Style.Font.Color.SetColor(Color.White);

                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    ws.Cells[row, 2].Value = sum;
                    ws.Cells[row, 1, row, 1].Merge = true;
                    ws.Cells[row, 1, row, 1].Value = "TOPLAM";

                    row += 2;

                    ws.Cells[row, 1].Value = "";
                    ws.Cells[row, 2].Value = "";
                    ws.Cells[row, 3].Value = "";
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

                ws.Cells[lastRow + 1, 2].Value = totalSum;

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn - 2].Merge = true;
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn - 2].Value = "GENEL TOPLAM";

                ws.Cells[1, 1, lastRow + 1, lastColumn].Style.Font.Size = 9;
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                ws.Cells[1, 1, lastRow + 1, lastColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[1, 1, lastRow + 1, lastColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Column(1).Style.WrapText = true;
                ws.Column(1).Width = 15;
                ws.Column(2).Width = 30;
                ws.Column(3).PageBreak = true;

                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Portrait;
                ws.PrinterSettings.Scale = 80;

                p.Save();
            }
            AddExportAudit(pageName, HttpContext?.User?.Identity?.Name, _context);
            return stream;
        }
    }
}
