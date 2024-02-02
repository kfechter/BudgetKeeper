using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetKeeper.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBudgetItemRepository BudgetItems { get; }

        int Complete();
    }
}
