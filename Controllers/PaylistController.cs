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
        public async Task<IActionResult> Post(bool state)
        {
            var paylistContext = await _context.Paylists
                .Where(e => e.State == state)
                .OrderBy(e => e.Date)
                .ThenByDescending(e => e.Amount)
                .AsNoTracking()
                .ToListAsync();

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

        public MemoryStream ExportPaylist(List<Paylists> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("Ödemeler");

                using (var range = ws.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "ID";
                ws.Cells[1, 2].Value = "Tarih";
                ws.Cells[1, 3].Value = "Tutar";
                ws.Cells[1, 4].Value = "Ödeme Yapılacak Kişi/Kurum";

                ws.Column(2).Style.Numberformat.Format = "dd-mmmm-yyyy";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Id;
                    ws.Cells[c, 2].Value = items[c - 2].Date;
                    ws.Cells[c, 3].Value = items[c - 2].Amount + " ₺";
                    ws.Cells[c, 4].Value = items[c - 2].PersonToBePaid;
                }

                var lastRow = ws.Dimension.End.Row;
                var lastColumn = ws.Dimension.End.Column;

                using (var range = ws.Cells[lastRow + 1, 1, lastRow + 1, lastColumn])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Gray);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[lastRow + 1, 2].Value = "Toplam:";
                ws.Cells[lastRow + 1, 3].Formula = String.Format("SUM(C2:C{0})", lastRow);

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells["A1:D" + items.Count + 2].AutoFilter = true;

                ws.Column(4).PageBreak = true;
                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Landscape;
                ws.PrinterSettings.Scale = 50;

                p.Save();
            }
            AddExportAudit(pageName, HttpContext?.User?.Identity?.Name, _context);
            return stream;
        }
    }
}
