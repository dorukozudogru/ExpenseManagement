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
    public class PointOfSaleController : Controller
    {
        private readonly ExpenseContext _context;

        public PointOfSaleController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sectors = _context.Sectors.Where(s => s.Name.Contains("SHELL")).ToList();
            sectors.Add(new Sectors
            {
                Id = 0,
                Name = ""
            });

            var banks = _context.BankBranches.ToList();
            banks.Add(new BankBranches
            {
                Id = 0,
                Name = ""
            });

            ViewData["SectorId"] = new SelectList(sectors.OrderBy(s => s.Id), "Id", "Name");
            ViewData["BankId"] = new SelectList(banks.OrderBy(t => t.Id), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, int year, int sectorId, int monthId, int bankId, double from, double to)
        {
            var requestFormData = Request.Form;

            var posContext = await _context.PointOfSale
                .Include(p => p.Sector)
                .Include(b => b.BankBranch)
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (year != 0)
                {
                    posContext = posContext.Where(e => e.Year == year).ToList();
                }
                if (sectorId != 0)
                {
                    posContext = posContext.Where(e => e.SectorId == sectorId).ToList();
                }
                if (monthId != 0)
                {
                    posContext = posContext.Where(e => e.Month == monthId).ToList();
                }
                if (bankId != 0)
                {
                    posContext = posContext.Where(e => e.BankBranchId == bankId).ToList();
                }
                if (from != 0 && to != 0)
                {
                    posContext = posContext.Where(e => e.Amount >= from && e.Amount <= to).ToList();
                }
            }

            foreach (var item in posContext)
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

            posContext = GetAllEnumNamesHelper.GetEnumName(posContext);

            List<PointOfSale> listItems = ProcessCollection(posContext, requestFormData);

            FakeSession.Instance.Obj = JsonConvert.SerializeObject(posContext);

            var response = new PaginatedResponse<PointOfSale>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = posContext.Count,
                RecordsTotal = posContext.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name");
            ViewData["SectorId"] = new SelectList(_context.Sectors.Where(s => s.Name.Contains("SHELL")), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PointOfSale pointOfSale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pointOfSale);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "POS Toplamı Başarıyla Kaydedilmiştir!" });
            }
            return BadRequest("POS Toplamı Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name");
            ViewData["SectorId"] = new SelectList(_context.Sectors.Where(s => s.Name.Contains("SHELL")), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PointOfSale pointOfSale)
        {
            var posContext = await _context.PointOfSale.FindAsync(id);

            if (posContext != null)
            {
                if (ModelState.IsValid)
                {
                    posContext.Year = pointOfSale.Year;
                    posContext.Month = pointOfSale.Month;
                    posContext.SectorId = pointOfSale.SectorId;
                    posContext.Date = pointOfSale.Date;
                    posContext.BankBranchId = pointOfSale.BankBranchId;
                    posContext.Amount = pointOfSale.Amount;
                    posContext.AmountCurrency = pointOfSale.AmountCurrency;

                    _context.Update(posContext);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "POS Toplamı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("POS Toplamı Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var posContext = await _context.PointOfSale
                .FirstOrDefaultAsync(m => m.Id == id);
            posContext = GetAllEnumNamesHelper.GetEnumName(posContext);

            if (posContext == null)
            {
                return View("Error");
            }

            return Ok(posContext);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var pointOfSale = await _context.PointOfSale
                .Include(p => p.BankBranch)
                .Include(p => p.Sector)
                .FirstOrDefaultAsync(m => m.Id == id);

            pointOfSale = GetAllEnumNamesHelper.GetEnumName(pointOfSale);

            if (pointOfSale == null)
            {
                return View("Error");
            }

            return View(pointOfSale);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pointOfSale = await _context.PointOfSale.FindAsync(id);
            _context.PointOfSale.Remove(pointOfSale);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "POS Toplamı Silinmiştir!" });
        }

        private bool PointOfSaleExists(int id)
        {
            return _context.PointOfSale.Any(e => e.Id == id);
        }
    }
}
