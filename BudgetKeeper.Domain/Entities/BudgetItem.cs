using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetKeeper.Domain.Entities
{

    public enum BudgetType
    {
        Loan,
        CreditCard,
        EIP,
        Other
    }

    public  class BudgetItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? DebtName { get; set; }

        public decimal? DebtAmount { get; set; }

        public decimal? MonthlyPayment { get; set; }

        [NotMapped]
        public decimal? TotalPayment {

            get
            {
                if(SubDebts!.Any())
                {
                    return SubDebts!.Sum(x => x.MonthlyPayment);
                }
                else
                {
                    return MonthlyPayment;
                }
            }
        }

        public List<SubDebt>? SubDebts { get; set; }

        public bool? PaidOff { get; set; }

        public BudgetType? BudgetType { get; set; }

        public bool IsOpen { get; set; }

        [NotMapped]
        public bool CloseCard { get; set; }

        [NotMapped]
        public int? PaymentsLeft
        {
            get
            {
                if(PaidOff.HasValue && PaidOff.Value)
                {
                    return 0;
                }

                if (DebtAmount.HasValue && MonthlyPayment.HasValue)
                {
                    return (int)Math.Ceiling(DebtAmount.Value / MonthlyPayment.Value);
                }
                return null;
            }
        }
    }
}
