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
using Microsoft.AspNetCore.Authorization;
using System.IO;
using static ExpenseManagement.Models.ViewModels.ReportViewModel;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = "Admin, Banaz, Muhasebe")]
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

        [HttpPost]
        public async Task<IActionResult> DailyMonthlyTotalPost()
        {
            var requestFormData = Request.Form;

            var energyDailies = await _context.EnergyDailies
                .GroupBy(i => new
                {
                    i.Date.Month
                })
                .Select(i => new GeneralResponse
                {
                    Month = i.Key.Month,
                    TotalAmount = i.Sum(x => x.Kw),
                })
                .ToListAsync();

            energyDailies = GetAllEnumNamesHelper.GetEnumName(energyDailies);

            var response = new PaginatedResponse<GeneralResponse>
            {
                Data = energyDailies,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = energyDailies.Count,
                RecordsTotal = energyDailies.Count
            };

            return Ok(response);
        }
        #endregion

        #region Monthly
        public IActionResult Monthly()
        {
            return View();
        }

        public async Task<IActionResult> MonthlyPost()
        {
            var requestFormData = Request.Form;

            var energyMonthlies = await _context.EnergyMonthlies
                .AsNoTracking()
                .ToListAsync();

            energyMonthlies = GetAllEnumNamesHelper.GetEnumName(energyMonthlies);

            List<EnergyMonthlies> listItems = ProcessCollection(energyMonthlies, requestFormData);

            var response = new PaginatedResponse<EnergyMonthlies>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = energyMonthlies.Count,
                RecordsTotal = energyMonthlies.Count
            };

            return Ok(response);
        }

        public IActionResult MonthlyCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MonthlyCreate([Bind("Id,Year,Month,ProducedKw,ConsumedKw,DistributionFee,Amount,TAX")] EnergyMonthlies energyMonthly)
        {
            if (ModelState.IsValid)
            {
                if (energyMonthly.Month != 0)
                {
                    _context.Add(energyMonthly);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Aylık Üretim Başarıyla Kaydedilmiştir!" });
                }
            }
            return BadRequest("Aylık Üretim Kaydedilirken Bir Hata Oluştu!");
        }

        public IActionResult MonthlyEdit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MonthlyEditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var energyMonthly = await _context.EnergyMonthlies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (energyMonthly == null)
            {
                return View("Error");
            }

            return Ok(energyMonthly);
        }

        [HttpPost]
        public async Task<IActionResult> MonthlyEdit(int id, [Bind("Id,Year,Month,ProducedKw,ConsumedKw,DistributionFee,Amount,TAX")] EnergyMonthlies energyMonthly)
        {
            var energyMonthlyFirst = await _context.EnergyMonthlies.FindAsync(id);

            if (energyMonthlyFirst != null)
            {
                if (ModelState.IsValid)
                {
                    energyMonthlyFirst.Year = energyMonthly.Year;
                    energyMonthlyFirst.Month = energyMonthly.Month;
                    energyMonthlyFirst.ProducedKw = energyMonthly.ProducedKw;
                    energyMonthlyFirst.ConsumedKw = energyMonthly.ConsumedKw;
                    energyMonthlyFirst.DistributionFee = energyMonthly.DistributionFee;
                    energyMonthlyFirst.Amount = energyMonthly.Amount;
                    energyMonthlyFirst.TAX = energyMonthly.TAX;

                    _context.Update(energyMonthlyFirst);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Aylık Üretim Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Aylık Üretim Güncellenirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> MonthlyDelete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var energyMonthly = await _context.EnergyMonthlies
                .FirstOrDefaultAsync(m => m.Id == id);

            energyMonthly = GetAllEnumNamesHelper.GetEnumName(energyMonthly);

            if (energyMonthly == null)
            {
                return View("Error");
            }

            return View(energyMonthly);
        }

        [HttpPost, ActionName("MonthlyDelete")]
        public async Task<IActionResult> MonthlyDeleteConfirmed(int id)
        {
            var energyMonthly = await _context.EnergyMonthlies.FindAsync(id);
            _context.EnergyMonthlies.Remove(energyMonthly);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Aylık Üretim Kaydı Silinmiştir!" });
        }
        #endregion

        public async Task<IActionResult> YearlyPost()
        {
            var energyMonthlies = await _context.EnergyMonthlies
                .AsNoTracking()
                .ToListAsync();

            var response = new List<double>
            {
                energyMonthlies.Sum(em => em.ProducedKw),
                energyMonthlies.Sum(em => em.ConsumedKw),
                energyMonthlies.Sum(em => em.DistributionFee),
                energyMonthlies.Sum(em => em.Amount),
                energyMonthlies.Sum(em => em.TAX)
            };

            return Ok(response);
        }

        #region Luytob
        public IActionResult Luytob()
        {
            return View();
        }

        public async Task<IActionResult> LuytobPost()
        {
            var requestFormData = Request.Form;

            var energyLuytobs = await _context.EnergyLuytobs
                .AsNoTracking()
                .ToListAsync();

            energyLuytobs = GetAllEnumNamesHelper.GetEnumName(energyLuytobs);

            List<EnergyLuytobs> listItems = ProcessCollection(energyLuytobs, requestFormData);

            var response = new PaginatedResponse<EnergyLuytobs>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = energyLuytobs.Count,
                RecordsTotal = energyLuytobs.Count
            };

            return Ok(response);
        }

        public IActionResult LuytobCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LuytobCreate(EnergyLuytobs energyLuytobs)
        {
            EnergyLuytobFiles files = new EnergyLuytobFiles();

            if (ModelState.IsValid)
            {
                if (energyLuytobs.Month != 0)
                {
                    if (Request.Form.Files.Count == 2)
                    {
                        for (int i = 0; i < Request.Form.Files.Count; i++)
                        {
                            if (Request.Form.Files[i].ContentType.Contains("pdf"))
                            {
                                MemoryStream ms = new MemoryStream();
                                Request.Form.Files[i].CopyTo(ms);

                                ms.Close();
                                ms.Dispose();

                                if (i == 0)
                                {
                                    files.Luytob = ms.ToArray();
                                    files.LuytobFormat = Request.Form.Files[i].ContentType;
                                }
                                if (i == 1)
                                {
                                    files.Invoice = ms.ToArray();
                                    files.InvoiceFormat = Request.Form.Files[i].ContentType;
                                }
                            }
                            else
                            {
                                return BadRequest("PDF Dosyası Ekleyin!");
                            }
                        }
                        _context.Add(files);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return BadRequest("Hem Lüytob Hem de Faturayı Yükleyiniz!");
                    }

                    energyLuytobs.EnergyLuytobFileId = files.Id;
                    _context.Add(energyLuytobs);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Lüytob/Fatura Başarıyla Kaydedilmiştir!" });
                }
            }
            return BadRequest("Lüytob/Fatura Kaydedilirken Bir Hata Oluştu!");
        }

        public async Task<IActionResult> LuytobDetails(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var energyLuytobs = await _context.EnergyLuytobs
                .Include(el => el.EnergyLuytobFile)
                .FirstOrDefaultAsync(m => m.Id == id);

            energyLuytobs = GetAllEnumNamesHelper.GetEnumName(energyLuytobs);

            if (energyLuytobs == null)
            {
                return View("Error");
            }

            return View(energyLuytobs);
        }

        public async Task<IActionResult> LuytobDelete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var energyLuytobs = await _context.EnergyLuytobs
                .Include(el => el.EnergyLuytobFile)
                .FirstOrDefaultAsync(m => m.Id == id);

            energyLuytobs = GetAllEnumNamesHelper.GetEnumName(energyLuytobs);

            if (energyLuytobs == null)
            {
                return View("Error");
            }

            return View(energyLuytobs);
        }

        [HttpPost, ActionName("LuytobDelete")]
        public async Task<IActionResult> LuytobDeleteConfirmed(int id)
        {
            var energyLuytobs = await _context.EnergyLuytobs.FindAsync(id);
            var energyLuytobFiles = await _context.EnergyLuytobFiles.FindAsync(energyLuytobs.EnergyLuytobFileId);
            _context.EnergyLuytobs.Remove(energyLuytobs);
            _context.EnergyLuytobFiles.Remove(energyLuytobFiles);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Lüytob/Fatura Kaydı Silinmiştir!" });
        }
        #endregion
    }
}