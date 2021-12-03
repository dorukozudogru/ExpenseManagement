using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using ExpenseManagement.Helpers;
using Microsoft.AspNetCore.Http;
using ExpenseManagement.Models.ViewModels;
using Newtonsoft.Json;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
    public class InterestIncomeController : Controller
    {
        private readonly ExpenseContext _context;

        public InterestIncomeController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var interestTypes = Enum.GetValues(typeof(InterestIncomes.InterestTypeEnum)).Cast<InterestIncomes.InterestTypeEnum>().ToList();
            List<string> types = new List<string>();

            foreach (var item in interestTypes)
            {
                types.Add(EnumExtensionsHelper.GetDisplayName(item));
            }
            types.Add("");

            ViewData["InterestType"] = new SelectList(types.OrderBy(t => t));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, int year, string interestType)
        {
            var requestFormData = Request.Form;

            var iContext = await _context.InterestIncomes
                .AsNoTracking()
                .ToListAsync();

            var Id = 999999;

            if (isFiltered != false)
            {
                if (year != 0)
                {
                    iContext = iContext.Where(e => e.Year == year).ToList();
                }
                if (interestType != null)
                {
                    var iTypes = Enum.GetValues(typeof(InterestIncomes.InterestTypeEnum)).Cast<InterestIncomes.InterestTypeEnum>().ToList();
                    for (int i = 0; i < iTypes.Count; i++)
                    {
                        if (EnumExtensionsHelper.GetDisplayName(iTypes[i]).ToString() == interestType)
                        {
                            Id = i;
                        }
                    }
                    iContext = iContext.Where(e => e.InterestType == Id).ToList();
                }
            }

            iContext = GetAllEnumNamesHelper.GetEnumName(iContext);

            List<InterestIncomes> listItems = ProcessCollection(iContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(iContext);

            var response = new PaginatedResponse<InterestIncomes>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = iContext.Count,
                RecordsTotal = iContext.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            var interestTypes = Enum.GetValues(typeof(InterestIncomes.InterestTypeEnum)).Cast<InterestIncomes.InterestTypeEnum>().ToList();
            List<string> types = new List<string>();

            foreach (var item in interestTypes)
            {
                types.Add(EnumExtensionsHelper.GetDisplayName(item));
            }

            ViewData["InterestType"] = new SelectList(types);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InterestIncomes interestIncomes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interestIncomes);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Faiz Geliri Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Faiz Geliri Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            var interestTypes = Enum.GetValues(typeof(InterestIncomes.InterestTypeEnum)).Cast<InterestIncomes.InterestTypeEnum>().ToList();
            List<string> types = new List<string>();

            foreach (var item in interestTypes)
            {
                types.Add(EnumExtensionsHelper.GetDisplayName(item));
            }

            ViewData["InterestType"] = new SelectList(types);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, InterestIncomes interestIncomes)
        {
            var iContext = await _context.InterestIncomes.FindAsync(id);

            if (iContext != null)
            {
                if (ModelState.IsValid)
                {
                    iContext.Year = interestIncomes.Year;
                    iContext.InterestType = interestIncomes.InterestType;
                    iContext.Amount = interestIncomes.Amount;

                    _context.Update(iContext);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Faiz Geliri Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Faiz Geliri Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var iContext = await _context.InterestIncomes
                .FirstOrDefaultAsync(m => m.Id == id);

            iContext = GetAllEnumNamesHelper.GetEnumName(iContext);

            if (iContext == null)
            {
                return View("Error");
            }

            return Ok(iContext);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var iContext = await _context.InterestIncomes
                .FirstOrDefaultAsync(m => m.Id == id);

            iContext = GetAllEnumNamesHelper.GetEnumName(iContext);

            if (iContext == null)
            {
                return View("Error");
            }

            return View(iContext);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interestIncomes = await _context.InterestIncomes.FindAsync(id);
            _context.InterestIncomes.Remove(interestIncomes);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Faiz Geliri Silinmiştir!" });
        }

        private bool InterestIncomesExists(int id)
        {
            return _context.InterestIncomes.Any(e => e.Id == id);
        }
    }
}
