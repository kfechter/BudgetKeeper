using BudgetKeeper.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetKeeper.Pages
{
    public class PayoffDebtModel(BudgetKeeperContext context) : PageModel
    {
        private readonly BudgetKeeperContext _context = context;

        public async Task<IActionResult> OnGet(int id)
        {
            var paidOffDebt = _context.BudgetItems!.Where(x => x.Id == id).FirstOrDefault();
            if (paidOffDebt != null)
            {
                paidOffDebt.PaidOff = true;
                _context.Update(paidOffDebt);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
