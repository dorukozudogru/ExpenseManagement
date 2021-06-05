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
using ExpenseManagement.Models.ViewModels;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe, Petrol"))]
    public class FuelSaleController : Controller
    {
        private readonly ExpenseContext _context;

        public FuelSaleController(ExpenseContext context)
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
        public async Task<IActionResult> Post(bool isFiltered, int fuelTypeId, int monthId, int sectorId, DateTime date)
        {
            var requestFormData = Request.Form;

            var fsContext = await _context.FuelSales
                .Include(s => s.Sector)
                .Include(p => p.FuelType)
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (fuelTypeId != 0)
                {
                    fsContext = fsContext.Where(e => e.FuelTypeId == fuelTypeId).ToList();
                }
                if (monthId != 0)
                {
                    fsContext = fsContext.Where(e => e.Date != null && e.Date.Month == monthId).ToList();
                }
                if (sectorId != 0)
                {
                    fsContext = fsContext.Where(e => e.SectorId == sectorId).ToList();
                }
                if (date != DateTime.MinValue)
                {
                    fsContext = fsContext.Where(e => e.Date == date).ToList();
                }
            }

            List<FuelSales> listItems = ProcessCollection(fsContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(fsContext);

            var response = new PaginatedResponse<FuelSales>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = fsContext.Count,
                RecordsTotal = fsContext.Count
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
        public async Task<IActionResult> Create(FuelSales fuelSales)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fuelSales);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Akaryakıt Satışı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Akaryakıt Satışı Oluşturulurken Bir Hata Oluştu!");
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

            var fuelSales = await _context.FuelSales
                .Include(f => f.FuelType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fuelSales == null)
            {
                return View("Error");
            }

            return Ok(fuelSales);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FuelSales fuelSales)
        {
            var fuelSale = await _context.FuelSales
               .Include(f => f.FuelType)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (fuelSale != null)
            {
                if (ModelState.IsValid)
                {
                    fuelSale.Date = fuelSales.Date;
                    fuelSale.FuelPurchase = fuelSales.FuelPurchase;
                    fuelSale.FuelSale = fuelSales.FuelSale;
                    fuelSale.FuelTypeId = fuelSales.FuelTypeId;
                    fuelSale.NetLiterPurchase = fuelSales.NetLiterPurchase;
                    fuelSale.NetLiterSale = fuelSales.NetLiterSale;
                    fuelSale.SectorId = fuelSales.SectorId;

                    _context.Update(fuelSale);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Akaryakıt Satışı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Akaryakıt Satışı Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var fuelSales = await _context.FuelSales
                .Include(f => f.FuelType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fuelSales == null)
            {
                return View("Error");
            }

            return View(fuelSales);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fuelSales = await _context.FuelSales.FindAsync(id);
            _context.FuelSales.Remove(fuelSales);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Akaryakıt Satışı Silinmiştir!" });
        }

        private bool FuelSalesExists(int id)
        {
            return _context.FuelSales.Any(e => e.Id == id);
        }
    }
}
