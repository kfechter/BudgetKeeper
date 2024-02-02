using BudgetKeeper.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
