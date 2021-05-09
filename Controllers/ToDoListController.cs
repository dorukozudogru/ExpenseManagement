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

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
    public class ToDoListController : Controller
    {
        private readonly ExpenseContext _context;

        public ToDoListController(ExpenseContext context)
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
            var toToListContext = await _context.ToDoLists
                .Where(e => e.State == state)
                .Include(e => e.Sector)
                .OrderByDescending(e => e.Id)
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe")
            {
                toToListContext = toToListContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            toToListContext = GetAllEnumNamesHelper.GetEnumName(toToListContext);

            return Ok(toToListContext);
        }

        public IActionResult Create()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,SectorId,Debtor,Amount,AmountCurrency")] ToDoLists toDoLists)
        {
            if (ModelState.IsValid)
            {
                toDoLists.State = false;
                toDoLists.CreatedAt = DateTime.Now;
                toDoLists.CreatedBy = GetLoggedUserId();

                _context.Add(toDoLists);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Alacak Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Alınacak Kaydedilirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var toDoLists = await _context.ToDoLists.FindAsync(id);
            toDoLists = GetAllEnumNamesHelper.GetEnumName(toDoLists);
            if (toDoLists == null)
            {
                return View("Error");
            }
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name", toDoLists.SectorId);
            return View(toDoLists);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ToDoLists toDoLists)
        {
            var toDoList = await _context.ToDoLists.FindAsync(id);

            if (toDoList != null)
            {
                if (ModelState.IsValid)
                {
                    toDoList.SectorId = toDoLists.SectorId;
                    toDoList.Debtor = toDoLists.Debtor;
                    toDoList.Amount = toDoLists.Amount;
                    toDoList.AmountCurrency = toDoLists.AmountCurrency;

                    _context.Update(toDoList);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Alacak Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Alacak Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditState(int? id)
        {
            var toDoList = await _context.ToDoLists.FindAsync(id);
            if (toDoList != null)
            {
                if (toDoList.State == true)
                {
                    toDoList.State = false;
                }
                else if (toDoList.State == false)
                {
                    toDoList.State = true;
                    toDoList.CollectAt = DateTime.Now;
                }

                _context.Update(toDoList);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Kayıt Başarıyla Güncellendi!" });
            }
            return BadRequest("Gider Güncellenirken Bir Hata Oluştu!");
        }

        public string GetLoggedUserId()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetLoggedUserRole()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ExportAllActiveToDos()
        {
            var stream = ExportToDo(_context.ToDoLists
                .Include(s => s.Sector)
                .Where(s => s.State == false).ToList(), "Alacak Listesi");
            string fileName = String.Format("{0}.xlsx", "Alacak Listesi");
            string fileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            stream.Position = 0;
            return File(stream, fileType, fileName);
        }

        [Authorize(Roles = "Admin")]
        public MemoryStream ExportToDo(List<ToDoLists> items, string pageName)
        {
            var stream = new System.IO.MemoryStream();

            items = GetAllEnumNamesHelper.GetEnumName(items);

            using (var p = new ExcelPackage(stream))
            {
                var ws = p.Workbook.Worksheets.Add("Poliçeler");

                using (var range = ws.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ws.Cells[1, 1].Value = "ID";
                ws.Cells[1, 2].Value = "Sektör/İş Kolu";
                ws.Cells[1, 3].Value = "Alınacak Kişi/Kurum";
                ws.Cells[1, 4].Value = "Tutar";
                ws.Cells[1, 5].Value = "Tahsilat Tarihi";

                ws.Column(5).Style.Numberformat.Format = "dd-mmmm-yyyy";

                ws.Row(1).Style.Font.Bold = true;

                for (int c = 2; c < items.Count + 2; c++)
                {
                    ws.Cells[c, 1].Value = items[c - 2].Id;
                    ws.Cells[c, 2].Value = items[c - 2].Sector.Name;
                    ws.Cells[c, 3].Value = items[c - 2].Debtor;
                    ws.Cells[c, 4].Value = items[c - 2].Amount + " " + items[c - 2].AmountCurrencyName;
                    ws.Cells[c, 5].Value = items[c - 2].CollectAt;

                    ws.Column(4).Style.Numberformat.Format = items[c - 2].AmountCurrencyName + "#,##0.00";
                }

                var lastRow = ws.Dimension.End.Row;
                var lastColumn = ws.Dimension.End.Column;

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.Cells["A1:E" + items.Count + 2].AutoFilter = true;

                ws.Column(5).PageBreak = true;
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
