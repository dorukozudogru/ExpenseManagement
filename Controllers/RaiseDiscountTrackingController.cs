using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using ExpenseManagement.Models.ViewModels;
using Newtonsoft.Json;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe, Petrol"))]
    public class RaiseDiscountTrackingController : Controller
    {
        private readonly ExpenseContext _context;

        public RaiseDiscountTrackingController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var fuelTypes = _context.FuelTypes.OrderBy(x => x.Name).ToList();
            fuelTypes.Add(new FuelTypes
            {
                Id = 0,
                Name = ""
            });

            var sectors = _context.Sectors.Where(s => s.Name.Contains("SHELL")).ToList();
            sectors.Add(new Sectors
            {
                Id = 0,
                Name = ""
            });

            ViewData["FuelTypeId"] = new SelectList(fuelTypes.OrderBy(s => s.Name), "Id", "Name");
            ViewData["SectorId"] = new SelectList(sectors.OrderBy(s => s.Id), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, int fuelTypeId, int monthId, int sectorId)
        {
            var requestFormData = Request.Form;

            var rdtContext = await _context.RaiseDiscountTracking
                .Include(s => s.Sector)
                .Include(p => p.FuelType)
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (fuelTypeId != 0)
                {
                    rdtContext = rdtContext.Where(e => e.FuelTypeId == fuelTypeId).ToList();
                }
                if (monthId != 0)
                {
                    rdtContext = rdtContext.Where(e => e.Date != null && e.Date.Month == monthId).ToList();
                }
                if (sectorId != 0)
                {
                    rdtContext = rdtContext.Where(e => e.SectorId == sectorId).ToList();
                }
            }

            List<RaiseDiscountTracking> listItems = ProcessCollection(rdtContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(rdtContext);

            var response = new PaginatedResponse<RaiseDiscountTracking>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = rdtContext.Count,
                RecordsTotal = rdtContext.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            ViewData["FuelTypeId"] = new SelectList(_context.FuelTypes.OrderBy(x => x.Name), "Id", "Name");
            ViewData["SectorId"] = new SelectList(_context.Sectors.Where(s => s.Name.Contains("SHELL")).OrderBy(s => s.Id), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RaiseDiscountTracking raiseDiscountTracking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raiseDiscountTracking);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Zam/İndirim Takibi Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Zam/İndirim Takibi Oluşturulurken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewData["FuelTypeId"] = new SelectList(_context.FuelTypes.OrderBy(x => x.Name), "Id", "Name");
            ViewData["SectorId"] = new SelectList(_context.Sectors.Where(s => s.Name.Contains("SHELL")).OrderBy(s => s.Id), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var rdtContext = await _context.RaiseDiscountTracking
                .Include(s => s.Sector)
                .Include(p => p.FuelType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rdtContext == null)
            {
                return View("Error");
            }

            return Ok(rdtContext);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RaiseDiscountTracking raiseDiscountTracking)
        {
            var rdtContext = await _context.RaiseDiscountTracking
                .Include(s => s.Sector)
                .Include(p => p.FuelType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rdtContext != null)
            {
                if (ModelState.IsValid)
                {
                    rdtContext.Date = raiseDiscountTracking.Date;
                    rdtContext.Difference = raiseDiscountTracking.Difference;
                    rdtContext.FuelRemainingAmount = raiseDiscountTracking.FuelRemainingAmount;
                    rdtContext.FuelTypeId = raiseDiscountTracking.FuelTypeId;
                    rdtContext.SectorId = raiseDiscountTracking.SectorId;
                    rdtContext.UnitSalePrice = raiseDiscountTracking.UnitSalePrice;

                    _context.Update(rdtContext);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Zam/İndirim Takibi Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Zam/İndirim Takibi Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var rdtContext = await _context.RaiseDiscountTracking
                .Include(r => r.FuelType)
                .Include(r => r.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rdtContext == null)
            {
                return View("Error");
            }

            return View(rdtContext);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rdtContext = await _context.RaiseDiscountTracking.FindAsync(id);
            _context.RaiseDiscountTracking.Remove(rdtContext);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Zam/İndirim Takibi Silinmiştir!" });
        }

        private bool RaiseDiscountTrackingExists(int id)
        {
            return _context.RaiseDiscountTracking.Any(e => e.Id == id);
        }
    }
}
