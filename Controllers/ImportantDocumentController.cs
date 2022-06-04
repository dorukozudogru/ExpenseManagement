using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using ExpenseManagement.Helpers;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using static ExpenseManagement.Helpers.AddExportAuditHelper;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using static ExpenseManagement.Models.ViewModels.ReportViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Security.Claims;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
    public class ImportantDocumentController : Controller
    {
        private readonly ExpenseContext _context;

        public ImportantDocumentController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Post(bool isFiltered, string definition)
        {
            var requestFormData = Request.Form;

            var importantDocuments = await _context.ImportantDocuments
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (definition != null)
                {
                    importantDocuments = importantDocuments.Where(e => e.Definition.ToUpper().Contains(definition.ToUpper())).ToList();
                }
            }

            List<ImportantDocuments> listItems = ProcessCollection(importantDocuments, requestFormData);

            var response = new PaginatedResponse<ImportantDocuments>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = importantDocuments.Count,
                RecordsTotal = importantDocuments.Count
            };

            return Ok(response);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var importantDocuments = await _context.ImportantDocuments
                .FirstOrDefaultAsync(m => m.Id == id);

            if (importantDocuments == null)
            {
                return View("Error");
            }

            return View(importantDocuments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ImportantDocuments importantDocuments)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count != 0)
                {
                    if (Request.Form.Files.First().ContentType.Contains("pdf") || Request.Form.Files.First().ContentType.Contains("image"))
                    {
                        MemoryStream ms = new MemoryStream();
                        Request.Form.Files.First().CopyTo(ms);

                        ms.Close();
                        ms.Dispose();

                        importantDocuments.Document = ms.ToArray();
                        importantDocuments.DocumentFormat = Request.Form.Files.First().ContentType;

                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        _context.Add(importantDocuments);
                        _context.SaveChanges();
                        return Ok(new { Result = true, Message = "Doküman Başarıyla Kaydedilmiştir!" });
                    }
                    else
                    {
                        return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                    }
                }
                return BadRequest("Lütfen Dosya Ekleyin!");
            }
            return BadRequest("Doküman Kaydedilirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var importantDocument = await _context.ImportantDocuments
                .FirstOrDefaultAsync(m => m.Id == id);

            if (importantDocument == null)
            {
                return View("Error");
            }

            return View(importantDocument);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var importantDocuments = await _context.ImportantDocuments.FindAsync(id);
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _context.ImportantDocuments.Remove(importantDocuments);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Doküman Silinmiştir!" });
        }
    }
}