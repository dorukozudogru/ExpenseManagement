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
    public class PetrolNetProfitController : Controller
    {
        private readonly ExpenseContext _context;

        public PetrolNetProfitController(ExpenseContext context)
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

            var pContext = await _context.PetrolNetProfit
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (year != 0)
                {
                    pContext = pContext.Where(e => e.Year == year).ToList();
                }
                if (monthId != 0)
                {
                    pContext = pContext.Where(e => e.Month == monthId).ToList();
                }
            }

            pContext = GetAllEnumNamesHelper.GetEnumName(pContext);

            List<PetrolNetProfit> listItems = ProcessCollection(pContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(pContext);

            var response = new PaginatedResponse<PetrolNetProfit>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = pContext.Count,
                RecordsTotal = pContext.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PetrolNetProfit petrolNetProfit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petrolNetProfit);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Net Kârlılık Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Net Kârlılık Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PetrolNetProfit petrolNetProfit)
        {
            var pContext = await _context.PetrolNetProfit.FindAsync(id);

            if (pContext != null)
            {
                if (ModelState.IsValid)
                {
                    pContext.Year = petrolNetProfit.Year;
                    pContext.Month = petrolNetProfit.Month;
                    pContext.GrossProfit = petrolNetProfit.GrossProfit;
                    pContext.Cost = petrolNetProfit.Cost;
                    pContext.NetProfit = petrolNetProfit.NetProfit;

                    _context.Update(pContext);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Net Kârlılık Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Net Kârlılık Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var pContext = await _context.PetrolNetProfit
                .FirstOrDefaultAsync(m => m.Id == id);
            pContext = GetAllEnumNamesHelper.GetEnumName(pContext);

            if (pContext == null)
            {
                return View("Error");
            }

            return Ok(pContext);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var pContext = await _context.PetrolNetProfit
                .FirstOrDefaultAsync(m => m.Id == id);

            pContext = GetAllEnumNamesHelper.GetEnumName(pContext);

            if (pContext == null)
            {
                return View("Error");
            }

            return View(pContext);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var petrolNetProfit = await _context.PetrolNetProfit.FindAsync(id);
            _context.PetrolNetProfit.Remove(petrolNetProfit);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Net Kârlılık Silinmiştir!" });
        }

        private bool PetrolNetProfitExists(int id)
        {
            return _context.PetrolNetProfit.Any(e => e.Id == id);
        }
    }
}
