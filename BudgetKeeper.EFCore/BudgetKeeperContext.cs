using BudgetKeeper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetKeeper.EFCore
{
    public class BudgetKeeperContext: DbContext
    {
        public BudgetKeeperContext(DbContextOptions<BudgetKeeperContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BudgetItem>? BudgetItems { get; set; }

        public DbSet<SubDebt>? SubDebt { get; set; }
    }
}
