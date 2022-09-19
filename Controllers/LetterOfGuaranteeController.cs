using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using System.Security.Claims;
using System.IO;
using System.Collections.Generic;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using Microsoft.AspNetCore.Authorization;
using System;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
    public class LetterOfGuaranteeController : Controller
    {
        private readonly ExpenseContext _context;

        public LetterOfGuaranteeController(ExpenseContext context)
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

            var letterOfGuarantee = await _context.LetterOfGuarantees
               .Include(c => c.BankBranch)
               .Include(b => b.LetterOfGuaranteeCosts)
               .AsNoTracking()
               .ToListAsync();

            List<LetterOfGuarantees> listItems = ProcessCollection(letterOfGuarantee, requestFormData);

            var response = new PaginatedResponse<LetterOfGuarantees>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = letterOfGuarantee.Count,
                RecordsTotal = letterOfGuarantee.Count
            };

            return Ok(response);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var letterOfGuarantee = await _context.LetterOfGuarantees
                .Include(l => l.BankBranch)
                .Include(l => l.LetterOfGuaranteeCosts)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (letterOfGuarantee == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz")
            {
                return View(letterOfGuarantee);
            }
            return View("AccessDenied");
        }

        public IActionResult Create()
        {
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name");
            ViewData["LetterOfGuaranteeCostId"] = new SelectList(_context.LetterOfGuaranteeCosts, "Id", "Id");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LetterOfGuarantees letterOfGuarantee)
        {
            LetterOfGuaranteeDocuments document = new LetterOfGuaranteeDocuments();

            if (ModelState.IsValid)
            {
                _context.Add(letterOfGuarantee);
                await _context.SaveChangesAsync();

                if (Request.Form.Files.Count != 0)
                {
                    if (Request.Form.Files.First().ContentType.Contains("pdf") || Request.Form.Files.First().ContentType.Contains("image"))
                    {
                        MemoryStream ms = new MemoryStream();
                        Request.Form.Files.First().CopyTo(ms);

                        ms.Close();
                        ms.Dispose();

                        document.LetterOfGuaranteeId = letterOfGuarantee.Id;
                        document.LetterOfGuaranteeImage = ms.ToArray();
                        document.LetterOfGuaranteeImageFormat = Request.Form.Files.First().ContentType;

                        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                        _context.Add(document);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                    }
                }
                return Ok(new { Result = true, Message = "Teminat Mektubu Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Teminat Mektubu Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name");
            ViewData["LetterOfGuaranteeCostId"] = new SelectList(_context.LetterOfGuaranteeCosts, "Id", "Id");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, LetterOfGuarantees letterOfGuarantees)
        {
            var letterOfGuarantee = await _context.LetterOfGuarantees.FindAsync(id);

            var oldDocument = await _context.LetterOfGuaranteeDocuments
                .FirstOrDefaultAsync(i => i.LetterOfGuaranteeId == id);

            LetterOfGuaranteeDocuments newDocument = new LetterOfGuaranteeDocuments();

            if (letterOfGuarantee != null)
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
                                newDocument.LetterOfGuaranteeId = letterOfGuarantee.Id;
                                newDocument.LetterOfGuaranteeImage = ms.ToArray();
                                newDocument.LetterOfGuaranteeImageFormat = Request.Form.Files.First().ContentType;

                                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                _context.Add(newDocument);
                                _context.SaveChanges();
                            }

                            if (oldDocument != null)
                            {
                                oldDocument.LetterOfGuaranteeId = letterOfGuarantee.Id;
                                oldDocument.LetterOfGuaranteeImage = ms.ToArray();
                                oldDocument.LetterOfGuaranteeImageFormat = Request.Form.Files.First().ContentType;

                                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                _context.Update(oldDocument);
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            return BadRequest("PDF veya Resim Dosyası Ekleyin!");
                        }
                    }

                    letterOfGuarantee.Institution = letterOfGuarantees.Institution;
                    letterOfGuarantee.BankBranchId = letterOfGuarantees.BankBranchId;
                    letterOfGuarantee.Amount = letterOfGuarantees.Amount;
                    letterOfGuarantee.LetterOfGuaranteeCostId = letterOfGuarantees.LetterOfGuaranteeCostId;
                    letterOfGuarantee.Cost = letterOfGuarantees.Cost;
                    letterOfGuarantee.FinishDate = letterOfGuarantees.FinishDate;

                    _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                    _context.Update(letterOfGuarantee);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Teminat Mektubu Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Teminat Mektubu Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var letterOfGuarantee = await _context.LetterOfGuarantees
                .FirstOrDefaultAsync(m => m.Id == id);

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz")
            {
                return Ok(letterOfGuarantee);
            }

            if (letterOfGuarantee == null)
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

            var document = await _context.LetterOfGuaranteeDocuments
                .FirstOrDefaultAsync(m => m.LetterOfGuaranteeId == id);

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

            var letterOfGuarantee = await _context.LetterOfGuarantees
                .Include(l => l.BankBranch)
                .Include(l => l.LetterOfGuaranteeCosts)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (letterOfGuarantee == null)
            {
                return View("Error");
            }

            if (GetLoggedUserRole() == "Admin" || GetLoggedUserRole() == "Muhasebe" || GetLoggedUserRole() == "Banaz")
            {
                return View(letterOfGuarantee);
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var document = await _context.LetterOfGuaranteeDocuments.FirstOrDefaultAsync(ed => ed.LetterOfGuaranteeId == id);
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _context.LetterOfGuaranteeDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Görüntü Silinmiştir!" });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var letterOfGuarantee = await _context.LetterOfGuarantees.FindAsync(id);
            _context.LetterOfGuarantees.Remove(letterOfGuarantee);
            await _context.SaveChangesAsync();

            var documents = await _context.LetterOfGuaranteeDocuments.FirstOrDefaultAsync(ed => ed.LetterOfGuaranteeId == id);
            if (documents != null)
            {
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                _context.LetterOfGuaranteeDocuments.Remove(documents);
                await _context.SaveChangesAsync();
            }
            return Ok(new { Result = true, Message = "Teminat Mektubu Silinmiştir!" });
        }

        public IActionResult GetInstitutions()
        {
            var name = HttpContext.Request.Query["term"].ToString();
            var institution = _context.LetterOfGuarantees.Where(c => c.Institution.Contains(name)).GroupBy(c => c.Institution).Select(c => c.Key).ToList();
            List<string> data = new List<string>();
            if (institution.Count != 0)
            {
                for (int i = 0; i < institution.Count; i++)
                {
                    if (i < 5)
                    {
                        data.Add(institution[i]);
                    }
                }
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> FinishingGuaranteeDate()
        {
            var myVehicles = await _context.LetterOfGuarantees
                .Where(m => m.FinishDate.AddDays(-20) <= DateTime.Now)
                .ToListAsync();

            var user = await _context.Users.FindAsync(GetLoggedUserId());

            if (user.FinishingGuaranteeClickDate.Value.Date != DateTime.Now.Date)
            {
                return Ok(myVehicles);
            }
            return Ok();
        }

        public string GetLoggedUserId()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetLoggedUserRole()
        {
            return this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }
    }
}
