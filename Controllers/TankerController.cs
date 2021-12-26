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
    [Authorize(Roles = ("Admin, Banaz, Muhasebe, Petrol"))]
    public class TankerController : Controller
    {
        private readonly ExpenseContext _context;

        public TankerController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, int year, int monthId)
        {
            var requestFormData = Request.Form;

            var tContext = await _context.Tankers
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (year != 0)
                {
                    tContext = tContext.Where(e => e.Year == year).ToList();
                }
                if (monthId != 0)
                {
                    tContext = tContext.Where(e => e.Month == monthId).ToList();
                }
            }

            tContext = GetAllEnumNamesHelper.GetEnumName(tContext);

            List<Tankers> listItems = ProcessCollection(tContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(tContext);

            var response = new PaginatedResponse<Tankers>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = tContext.Count,
                RecordsTotal = tContext.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tankers tankers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tankers);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Tanker Gönderimi Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Tanker Gönderimi Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Tankers tankers)
        {
            var tContext = await _context.Tankers.FindAsync(id);

            if (tContext != null)
            {
                if (ModelState.IsValid)
                {
                    tContext.Year = tankers.Year;
                    tContext.Month = tankers.Month;
                    tContext.TankerAmount = tankers.TankerAmount;

                    _context.Update(tContext);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Tanker Gönderimi Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Tanker Gönderimi Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var tContext = await _context.Tankers
                .FirstOrDefaultAsync(m => m.Id == id);
            tContext.MonthName = GetMonthNameHelper.GetMonth(tContext.Month);

            if (tContext == null)
            {
                return View("Error");
            }

            return Ok(tContext);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var tContext = await _context.Tankers
                .FirstOrDefaultAsync(m => m.Id == id);

            tContext.MonthName = GetMonthNameHelper.GetMonth(tContext.Month);

            if (tContext == null)
            {
                return View("Error");
            }

            return View(tContext);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tankers = await _context.Tankers.FindAsync(id);
            _context.Tankers.Remove(tankers);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Tanker Gönderimi Silinmiştir!" });
        }

        private bool TankersExists(int id)
        {
            return _context.Tankers.Any(e => e.Id == id);
        }
    }
}
