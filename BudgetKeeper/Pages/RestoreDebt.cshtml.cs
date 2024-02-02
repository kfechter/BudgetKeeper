using BudgetKeeper.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetKeeper.Pages
{
    public class RestoreDebtModel(BudgetKeeperContext context) : PageModel
    {
        private readonly BudgetKeeperContext _context = context;

        public async Task<IActionResult> OnGet(int id)
        {
            var paidOffDebtToRestore = _context.BudgetItems!.Where(x => x.Id == id).FirstOrDefault();
            if (paidOffDebtToRestore != null)
            {
                paidOffDebtToRestore.PaidOff = false;
                _context.Update(paidOffDebtToRestore);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
