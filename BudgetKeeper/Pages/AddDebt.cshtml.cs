using BudgetKeeper.Domain.Entities;
using BudgetKeeper.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetKeeper.Pages
{
    public class AddDebtModel(BudgetKeeperContext context) : PageModel
    {
        private readonly BudgetKeeperContext _context = context;

        [BindProperty]
        public BudgetItem? NewDebt { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(NewDebt != null)
            {
                NewDebt.PaidOff = false; //Should be false, but making sure
                _context.BudgetItems!.Add(NewDebt);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
