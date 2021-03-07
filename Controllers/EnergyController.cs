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
using static ExpenseManagement.Helpers.ProcessCollectionHelper;

namespace ExpenseManagement.Controllers
{
    public class EnergyController : Controller
    {
        private readonly ExpenseContext _context;

        public EnergyController(ExpenseContext context)
        {
            _context = context;
        }

        #region Daily
        public IActionResult Daily()
        {
            return View();
        }

        public async Task<IActionResult> DailyPost()
        {
            var requestFormData = Request.Form;

            var energyDailies = await _context.EnergyDailies
                .AsNoTracking()
                .ToListAsync();

            List<EnergyDaily> listItems = ProcessCollection(energyDailies, requestFormData);

            var response = new PaginatedResponse<EnergyDaily>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = energyDailies.Count,
                RecordsTotal = energyDailies.Count
            };

            return Ok(response);
        }

        public IActionResult DailyCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DailyCreate([Bind("Id,Date,Kw")] EnergyDaily energyDaily)
        {
            if (ModelState.IsValid)
            {
                _context.Add(energyDaily);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Günlük Üretim Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("Günlük Üretim Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult DailyEdit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DailyEditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var energyDaily = await _context.EnergyDailies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (energyDaily == null)
            {
                return View("Error");
            }

            return Ok(energyDaily);
        }

        [HttpPost]
        public async Task<IActionResult> DailyEdit(int id, [Bind("Id,Date,Kw")] EnergyDaily energyDaily)
        {
            var energyDailyFirst = await _context.EnergyDailies.FindAsync(id);

            if (energyDailyFirst != null)
            {
                if (ModelState.IsValid)
                {
                    energyDailyFirst.Date = energyDaily.Date;
                    energyDailyFirst.Kw = energyDaily.Kw;
                    
                    _context.Update(energyDailyFirst);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Günlük Üretim Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Günlük Üretim Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> DailyDelete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var energyDaily = await _context.EnergyDailies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (energyDaily == null)
            {
                return View("Error");
            }

            return View(energyDaily);
        }

        [HttpPost, ActionName("DailyDelete")]
        public async Task<IActionResult> DailyDeleteConfirmed(int id)
        {
            var energyDaily = await _context.EnergyDailies.FindAsync(id);
            _context.EnergyDailies.Remove(energyDaily);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Günlük Üretim Kaydı Silinmiştir!" });
        }
        #endregion
    }
}
