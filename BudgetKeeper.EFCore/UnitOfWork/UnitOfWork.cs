using BudgetKeeper.Domain.Interfaces;
using BudgetKeeper.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetKeeper.EFCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BudgetKeeperContext _context;

        public UnitOfWork(BudgetKeeperContext context)
        {
            _context = context;
            BudgetItems = new BudgetItemRepository(_context);
            SubDebt = new SubDebtRepository(_context);
        }

        public IBudgetItemRepository BudgetItems { get; private set; }

        public ISubDebtRepository SubDebt {  get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
