using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetKeeper.Domain.Entities
{
    public  class BudgetItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? DebtName { get; set; }

        public decimal? DebtAmount { get; set; }

        public decimal? MonthlyPayment { get; set; }

        public bool? PaidOff { get; set; }
    }
}
