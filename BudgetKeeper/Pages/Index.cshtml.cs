using BudgetKeeper.Domain.Entities;
using BudgetKeeper.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

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

        private async Task LoadData()
        {
            CurrentDebts = await _context.BudgetItems!.Where(x => !x.PaidOff!.Value).OrderBy(x => x.Id).ToListAsync();
            PastDebts = await _context.BudgetItems!.Where(x =>  x.PaidOff!.Value).OrderBy(x => x.Id).ToListAsync();
            TotalUnpaidDebt = CurrentDebts.Sum(x => x.DebtAmount!.Value);
            TotalMonthlyPayment = CurrentDebts.Sum(x => x.MonthlyPayment!.Value);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateDebtSaveAsync(string debtName, decimal debtAmount, decimal monthlyPayment)
        {
            var debt = new BudgetItem
            {
                DebtName = debtName,
                DebtAmount = debtAmount,
                MonthlyPayment = monthlyPayment,
                PaidOff = false
            };
            await _context.BudgetItems!.AddAsync(debt);
            await _context.SaveChangesAsync();
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostPayoffSaveItemAsync(int id)
        {
            var debt = await _context.BudgetItems!.FindAsync(id);
            debt!.PaidOff = true;
            _context.BudgetItems!.Update(debt);
            await _context.SaveChangesAsync();
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostSaveItemAsync(int id, string debtName, decimal debtAmount, decimal monthlyPayment)
        {
            var debt = await _context.BudgetItems!.FindAsync(id);
            debt!.DebtName = debtName;
            debt!.DebtAmount = debtAmount;
            debt!.MonthlyPayment = monthlyPayment;
            await _context.SaveChangesAsync();
            await LoadData();
            return Page();
        }

        public async Task<PartialViewResult> OnPostPayoffItemAsync(int id)
        {
            return new PartialViewResult
            {
                ViewName = "Dialogs/PayoffDebtConfirmationModal",
                ViewData = new ViewDataDictionary<BudgetItem>(ViewData, await _context.BudgetItems!.FindAsync(id))
            };
        }

        public async Task<PartialViewResult> OnPostEditItemAsync(int id)
        {
            return new PartialViewResult
            {
                ViewName = "Dialogs/EditItemModal",
                ViewData = new ViewDataDictionary<BudgetItem>(ViewData, await _context.BudgetItems!.FindAsync(id))
            };
        }

        public PartialViewResult OnPostAddItem()
        {
            return new PartialViewResult
            {
                ViewName = "Dialogs/CreateItemModal",
                ViewData = new ViewDataDictionary<BudgetItem>(ViewData, new BudgetItem())
            };
        }
    }
}
