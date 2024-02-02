using BudgetKeeper.Domain.Entities;
using BudgetKeeper.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetKeeper.Pages
{
    public class EditDebtModel(BudgetKeeperContext context) : PageModel
    {
        private readonly BudgetKeeperContext _context = context;

        [BindProperty]
        public BudgetItem? Debt { get; set; }

        public IActionResult OnGet(int id)
        {
            Debt = _context.BudgetItems!.Where(x => x.Id == id).FirstOrDefault();
            if(Debt == null)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Debt != null)
            {
                Debt.PaidOff = false; //Should be false, but making sure
                _context.BudgetItems!.Update(Debt);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
