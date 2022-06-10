using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Data;
using ExpenseManagement.Models;
using ExpenseManagement.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
    public class BankLoanController : Controller
    {
        private readonly ExpenseContext _context;

        public BankLoanController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var requestFormData = Request.Form;

            var bankLoan = await _context.BankLoans
               .Include(c => c.BankBranch)
               .AsNoTracking()
               .ToListAsync();

            List<BankLoans> listItems = ProcessCollection(bankLoan, requestFormData);

            var response = new PaginatedResponse<BankLoans>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = bankLoan.Count,
                RecordsTotal = bankLoan.Count
            };

            return Ok(response);
        }

        public IActionResult Create()
        {
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BankLoans bankLoans)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bankLoans);
                await _context.SaveChangesAsync();
                return Ok(new { Result = true, Message = "Banka Kredisi Başarıyla Oluşturulmuştur!" });
            }
            return BadRequest("Banka Kredisi Oluşturulurken Bir Hata Oluştu!");
        }

        public IActionResult Edit()
        {
            ViewData["BankBranchId"] = new SelectList(_context.BankBranches, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BankLoans bankLoans)
        {
            var bankLoan = await _context.BankLoans.FindAsync(id);

            if (bankLoan != null)
            {
                if (ModelState.IsValid)
                {
                    bankLoan.BankBranchId = bankLoans.BankBranchId;
                    bankLoan.Amount = bankLoans.Amount;
                    bankLoan.InstallmentAmount = bankLoans.InstallmentAmount;
                    bankLoan.TotalAmountToBeRepaid = bankLoans.TotalAmountToBeRepaid;
                    bankLoan.Interest = bankLoans.Interest;
                    bankLoan.StartDate = bankLoans.StartDate;
                    bankLoan.FinishDate = bankLoans.FinishDate;

                    _context.Update(bankLoan);
                    await _context.SaveChangesAsync();
                    return Ok(new { Result = true, Message = "Banka Kredisi Başarıyla Güncellendi!" });
                }
                else
                    return BadRequest("Tüm Alanları Doldurunuz!");
            }
            return BadRequest("Banka Kredisi Güncellenirken Bir Hata Oluştu!");
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var bankLoans = await _context.BankLoans
                .Include(b => b.BankBranch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bankLoans == null)
            {
                return View("Error");
            }

            return Ok(bankLoans);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var bankLoans = await _context.BankLoans
                .Include(b => b.BankBranch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankLoans == null)
            {
                return View("Error");
            }

            return View(bankLoans);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankLoans = await _context.BankLoans.FindAsync(id);
            _context.BankLoans.Remove(bankLoans);
            await _context.SaveChangesAsync();
            return Ok(new { Result = true, Message = "Banka Kredisi Silinmiştir!" });
        }

        private bool BankLoansExists(int id)
        {
            return _context.BankLoans.Any(e => e.Id == id);
        }
    }
}