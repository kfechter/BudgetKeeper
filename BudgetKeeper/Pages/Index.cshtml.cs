using BudgetKeeper.Domain.Entities;
using BudgetKeeper.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BudgetKeeper.Pages
{
    public class IndexModel(ILogger<IndexModel> logger, BudgetKeeperContext context) : PageModel
    {
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly BudgetKeeperContext _context = context;

        public List<BudgetItem> CurrentDebts { get; set; } = [];

        public List<BudgetItem> PastDebts { get; set; } = [];

        public decimal TotalUnpaidDebt { get; set; }

        public decimal TotalMonthlyPayment { get; set; }

        public async Task OnGetAsync()
        {
            CurrentDebts = await _context.BudgetItems!.Where(x => !x.PaidOff!.Value).ToListAsync();
            PastDebts = await _context.BudgetItems!.Where(x =>  x.PaidOff!.Value).ToListAsync();
            TotalUnpaidDebt = CurrentDebts.Sum(x => x.DebtAmount!.Value);
            TotalMonthlyPayment = CurrentDebts.Sum(x => x.MonthlyPayment!.Value);
        }

        public async Task<IActionResult> OnPostPayOffDebtAsync(int id) 
        {
            var debt = await _context.BudgetItems!.FindAsync(id);
            debt!.PaidOff = true;
            _context.BudgetItems!.Update(debt);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRestoreDebtAsync(int id) 
        {
            var debt = await _context.BudgetItems!.FindAsync(id);
            debt!.PaidOff = false;
            _context.BudgetItems!.Update(debt);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
