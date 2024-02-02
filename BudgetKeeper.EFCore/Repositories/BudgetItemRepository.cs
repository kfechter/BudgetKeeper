using BudgetKeeper.Domain.Entities;
using BudgetKeeper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetKeeper.EFCore.Repositories
{
    public class BudgetItemRepository : GenericRepository<BudgetItem>, IBudgetItemRepository
    {
        public BudgetItemRepository(BudgetKeeperContext context)
            : base(context)
        {
        }
    }
}
