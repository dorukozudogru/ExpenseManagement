using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using ExpenseManagement.Helpers;
using System;
using static ExpenseManagement.Models.Expenses;
using System.Security.Claims;
using System.IO;
using System.Collections.Generic;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace ExpenseManagement.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ExpenseContext _context;

        public ExpenseController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public IActionResult Salary()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public IActionResult Paylist()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public async Task<IActionResult> SalaryPost()
        {
            var requestFormData = Request.Form;

            var expenseContext = await _context.Expenses
                .Where(e => e.ExpenseType == 2)
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                expenseContext = expenseContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            List<Expenses> listItems = ProcessCollection(expenseContext, requestFormData);

            var response = new PaginatedResponse<Expenses>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = expenseContext.Count,
                RecordsTotal = expenseContext.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var requestFormData = Request.Form;

            var expenseContext = await _context.Expenses
                .Where(e => e.ExpenseType != 2)
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                expenseContext = expenseContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            foreach (var item in expenseContext)
            {
                if (item.Sector != null)
                {
                    item.SectorName = item.Sector.Name;
                }
                if (item.Sector == null)
                {
                    item.SectorName = "EMPTY";
                }
            }

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            List<Expenses> listItems = ProcessCollection(expenseContext, requestFormData);

            var response = new PaginatedResponse<Expenses>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = expenseContext.Count,
                RecordsTotal = expenseContext.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> WeeklyPaylistPost()
        {
            var requestFormData = Request.Form;

            DateTime baseDate = DateTime.Today;
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek + 1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);

            var expenseContext = await _context.Expenses
                .Where(e => e.ExpenseType != 2 &&
                            e.LastPaymentDate != null &&
                            e.LastPaymentDate >= thisWeekStart &&
                            e.LastPaymentDate <= thisWeekEnd)
                .Include(e => e.Sector)
                .AsNoTracking()
                .ToListAsync();

            if (GetLoggedUserRole() != "Admin" && GetLoggedUserRole() != "Muhasebe" && GetLoggedUserRole() != "Banaz")
            {
                expenseContext = expenseContext
                    .Where(e => e.CreatedBy == GetLoggedUserId())
                    .ToList();
            }

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            List<Expenses> listItems = ProcessCollection(expenseContext, requestFormData);

            var response = new PaginatedResponse<Expenses>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = expenseContext.Count,
                RecordsTotal = expenseContext.Count
            };

            return Ok(response);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var expenses = await _context.Expenses
                .Include(e => e.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);

            expenses = GetAllEnumNamesHelper.GetEnumName(expenses);

            if (expenses == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || expenses.CreatedBy == GetLoggedUserId())
            {
                return View(expenses);
            }
            return View("AccessDenied");
        }

        public async Task<IActionResult> Print(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var documents = await _context.ExpenseDocuments
                .FirstOrDefaultAsync(m => m.ExpenseId == id);

            if (documents == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz")
            {
                return View(documents);
            }
            return View("AccessDenied");
        }

        public IActionResult Create()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expenses expenses)
        {
            ExpenseDocuments document = new ExpenseDocuments();

            if (ModelState.IsValid)
            {
                expenses.State = (int)StateEnum.Active;
                expenses.CreatedAt = DateTime.Now;
                expenses.CreatedBy = GetLoggedUserId();

                _context.Add(expenses);
                await _context.SaveChangesAsync();

                if (Request.Form.Files.Count != 0)
                {
                    if (Request.Form.Files.First().ContentType.Contains("pdf") || Request.Form.Files.First().ContentType.Contains("image"))
                    {
                        MemoryStream ms = new MemoryStream();
                        Request.Form.Files.First().CopyTo(ms);

                        ms.Close();
                        ms.Dispose();

                        document.ExpenseId = expenses.Id;
                        document.InvoiceImage = ms.ToArray();
                        document.InvoiceImageFormat = Request.Form.Files.First().ContentType;

                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        _context.Add(document);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                    }
                }
                return Ok(new { Result = true, Message = "Gider Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Gider Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Expenses expenses)
        {
            var expense = await _context.Expenses.FindAsync(id);

            var oldDocument = await _context.ExpenseDocuments
                .FirstOrDefaultAsync(i => i.ExpenseId == id);

            ExpenseDocuments newDocument = new ExpenseDocuments();

            if (expense != null)
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

                            if (oldDocument == null)
                            {
                                newDocument.ExpenseId = expense.Id;
                                newDocument.InvoiceImage = ms.ToArray();
                                newDocument.InvoiceImageFormat = Request.Form.Files.First().ContentType;

                                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                _context.Add(newDocument);
                                await _context.SaveChangesAsync();
                            }

                            if (oldDocument != null)
                            {
                                oldDocument.ExpenseId = expense.Id;
                                oldDocument.InvoiceImage = ms.ToArray();
                                oldDocument.InvoiceImageFormat = Request.Form.Files.First().ContentType;

                                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                _context.Update(oldDocument);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                        }
                    }

                    expense.ExpenseType = expenses.ExpenseType;
                    expense.SectorId = expenses.SectorId;
                    expense.SupplierDef = expenses.SupplierDef;
                    expense.Definition = expenses.Definition;
                    expense.Date = expenses.Date;
                    expense.LastPaymentDate = expenses.LastPaymentDate;
                    expense.Amount = expenses.Amount;
                    expense.AmountCurrency = expenses.AmountCurrency;
                    expense.TAX = expenses.TAX;
                    expense.TAXCurrency = expenses.TAXCurrency;

                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Gider Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Gider Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            expense = GetAllEnumNamesHelper.GetEnumName(expense);

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || expense.CreatedBy == GetLoggedUserId())
            {
                return Ok(expense);
            }

            if (expense == null)
            {
                return View("Error");
            }

            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> GetDocument(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var document = await _context.ExpenseDocuments
                .FirstOrDefaultAsync(m => m.ExpenseId == id);

            if (document == null)
            {
                return View("Error");
            }

            return Ok(document);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var expenses = await _context.Expenses
                .Include(e => e.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);
            expenses = GetAllEnumNamesHelper.GetEnumName(expenses);

            if (expenses == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz" || expenses.CreatedBy == GetLoggedUserId())
            {
                return View(expenses);
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var documents = await _context.ExpenseDocuments.FirstOrDefaultAsync(ed => ed.ExpenseId == id);
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _context.ExpenseDocuments.Remove(documents);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Görüntü Silinmiştir!" });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expenses = await _context.Expenses.FindAsync(id);
            _context.Expenses.Remove(expenses);
            await _context.SaveChangesAsync();

            var documents = await _context.ExpenseDocuments.FirstOrDefaultAsync(ed => ed.ExpenseId == id);
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _context.ExpenseDocuments.Remove(documents);
            await _context.SaveChangesAsync();

            return Ok(new { Result = true, Message = "Gider Silinmiştir!" });
        }

        public string GetLoggedUserId()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetLoggedUserRole()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }

        private bool ExpensesExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
