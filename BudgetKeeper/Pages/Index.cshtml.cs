using BudgetKeeper.Domain.Entities;
using BudgetKeeper.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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

        public async Task<IActionResult> OnPostCreateDebtSaveAsync(string debtName, decimal debtAmount, decimal monthlyPayment, BudgetType budgetType, bool isOpen)
        {
            var debt = new BudgetItem
            {
                DebtName = debtName,
                DebtAmount = debtAmount,
                MonthlyPayment = monthlyPayment,
                PaidOff = false,
                BudgetType = budgetType,
                IsOpen = isOpen
            };
            await _context.BudgetItems!.AddAsync(debt);
            await _context.SaveChangesAsync();
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostPayoffSaveItemAsync(int id, bool? closeOnPayoff)
        {
            var debt = await _context.BudgetItems!.FindAsync(id);
            debt!.PaidOff = true;
            debt.DebtAmount = 0;
            if(debt.BudgetType != BudgetType.CreditCard)
            {
                debt.IsOpen = false;
            }

            if (closeOnPayoff.HasValue && closeOnPayoff.Value)
            {
                debt.IsOpen = false;
            }

            _context.BudgetItems!.Update(debt);
            await _context.SaveChangesAsync();
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostSaveItemAsync(int id, string debtName, decimal debtAmount, decimal monthlyPayment, BudgetType budgetType, bool isOpen)
        {
            var debt = await _context.BudgetItems!.FindAsync(id);
            debt!.DebtName = debtName;
            debt!.DebtAmount = debtAmount;
            debt!.MonthlyPayment = monthlyPayment;
            debt!.BudgetType = budgetType;
            debt!.IsOpen = isOpen;
            _context.BudgetItems!.Update(debt);
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

        public async Task<IActionResult> OnPostCloseCardAsync(int id)
        {
            var debt = await _context.BudgetItems!.FindAsync(id);
            debt!.IsOpen = false;
            _context.BudgetItems!.Update(debt);
            await _context.SaveChangesAsync();
            await LoadData();
            return Page();
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

        public async Task<IActionResult> OnPostMakePaymentAsync(int id)
        {
            var debt = await _context.BudgetItems!.FindAsync(id);
            if(debt!.PaymentsLeft == 1)
            {
                return await OnPostPayoffSaveItemAsync(id, null);
            }

            debt!.DebtAmount -= debt!.MonthlyPayment;
            _context.BudgetItems!.Update(debt);
            await _context.SaveChangesAsync();
            await LoadData();
            return Page();
        }
    }
}
