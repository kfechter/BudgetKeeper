using BudgetKeeper.Domain.Entities;
using BudgetKeeper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetKeeper.EFCore.Repositories
{
    public class SubDebtRepository: GenericRepository<SubDebt>, ISubDebtRepository
    {
        public SubDebtRepository(BudgetKeeperContext context)
            : base(context)
        {
        }
    }
}
