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

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
    public class DepositAccountController : Controller
    {
        private readonly ExpenseContext _context;

        public DepositAccountController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isActive)
        {
            var requestFormData = Request.Form;

            var expenseContext = await _context.DepositAccounts
                .Where(d => d.IsActive == isActive)
                .Include(d => d.BankBranch)
                .AsNoTracking()
                .ToListAsync();

            if (isActive == true)
            {
                foreach (var item in expenseContext)
                {
                    if (item.FinishDate < DateTime.Now.Date)
                    {
                        item.IsActive = false;
                        _context.Update(item);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            expenseContext = await _context.DepositAccounts
                .Where(d => d.IsActive == isActive)
                .Include(d => d.BankBranch)
                .AsNoTracking()
                .ToListAsync();

            expenseContext = GetAllEnumNamesHelper.GetEnumName(expenseContext);

            List<DepositAccounts> listItems = ProcessCollection(expenseContext, requestFormData);

            var response = new PaginatedResponse<DepositAccounts>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = expenseContext.Count,
                RecordsTotal = expenseContext.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepositAccounts depositAccounts)
        {
            if (ModelState.IsValid)
            {
                if (depositAccounts.FinishDate < DateTime.Now.Date)
                {
                    depositAccounts.IsActive = false;
                }
                if (depositAccounts.FinishDate >= DateTime.Now.Date)
                {
                    depositAccounts.IsActive = true;
                }
                _context.Add(depositAccounts);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Vadeli Hesap Kaydı Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Vadeli Hesap Kaydı Oluşturulurken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DepositAccounts depositAccounts)
        {
            var depositAccount = await _context.DepositAccounts.FindAsync(id);

            if (depositAccount != null)
            {
                if (ModelState.IsValid)
                {
                    depositAccount.BankBranchId = depositAccounts.BankBranchId;
                    depositAccount.Amount = depositAccounts.Amount;
                    depositAccount.AmountCurrency = depositAccounts.AmountCurrency;
                    depositAccount.DayOfDeposit = depositAccounts.DayOfDeposit;
                    depositAccount.Interest = depositAccounts.Interest;
                    depositAccount.Profit = depositAccounts.Profit;
                    depositAccount.StartDate = depositAccounts.StartDate;
                    depositAccount.FinishDate = depositAccounts.FinishDate;

                    if (depositAccounts.FinishDate < DateTime.Now.Date)
                    {
                        depositAccount.IsActive = false;
                    }
                    if (depositAccounts.FinishDate >= DateTime.Now.Date)
                    {
                        depositAccount.IsActive = true;
                    }

                    _context.Update(depositAccount);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Vadeli Hesap Kaydı Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Vadeli Hesap Kaydı Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var depositAccounts = await _context.DepositAccounts
                .Include(d => d.BankBranch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (depositAccounts == null)
            {
                return View("Error");
            }

            return Ok(depositAccounts);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depositAccounts = await _context.DepositAccounts
                .Include(d => d.BankBranch)
                .FirstOrDefaultAsync(m => m.Id == id);
            depositAccounts = GetAllEnumNamesHelper.GetEnumName(depositAccounts);
            if (depositAccounts == null)
            {
                return NotFound();
            }

            return View(depositAccounts);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var depositAccounts = await _context.DepositAccounts.FindAsync(id);
            _context.DepositAccounts.Remove(depositAccounts);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Vadeli Hesap Kaydı Silinmiştir!" });
        }

        [HttpPost]
        public async Task<IActionResult> FinishDeposit(int id)
        {
            var depositAccounts = await _context.DepositAccounts.FindAsync(id);
            depositAccounts.IsActive = false;
            _context.Update(depositAccounts);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Vadeli Hesap Kaydı Bitirilmiştir!" });
        }
    }
}
