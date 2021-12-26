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
    public class WashingIncomeController : Controller
    {
        private readonly ExpenseContext _context;

        public WashingIncomeController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, int year)
        {
            var requestFormData = Request.Form;

            var wContext = await _context.WashingIncome
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (year != 0)
                {
                    wContext = wContext.Where(e => e.Year == year).ToList();
                }
            }

            List<WashingIncome> listItems = ProcessCollection(wContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(wContext);

            var response = new PaginatedResponse<WashingIncome>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = wContext.Count,
                RecordsTotal = wContext.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WashingIncome washingIncome)
        {
            if (ModelState.IsValid)
            {
                _context.Add(washingIncome);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Yıkama Geliri Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Yıkama Geliri Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, WashingIncome washingIncome)
        {
            var wContext = await _context.WashingIncome.FindAsync(id);

            if (wContext != null)
            {
                if (ModelState.IsValid)
                {
                    wContext.Year = washingIncome.Year;
                    wContext.GrossProfit = washingIncome.GrossProfit;
                    wContext.Cost = washingIncome.Cost;
                    wContext.NetProfit = washingIncome.NetProfit;

                    _context.Update(wContext);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Yıkama Geliri Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Yıkama Geliri Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var wContext = await _context.WashingIncome
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wContext == null)
            {
                return View("Error");
            }

            return Ok(wContext);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var wContext = await _context.WashingIncome
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wContext == null)
            {
                return View("Error");
            }

            return View(wContext);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var washingIncome = await _context.WashingIncome.FindAsync(id);
            _context.WashingIncome.Remove(washingIncome);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Yıkama Geliri Silinmiştir!" });
        }

        private bool WashingIncomeExists(int id)
        {
            return _context.WashingIncome.Any(e => e.Id == id);
        }
    }
}
