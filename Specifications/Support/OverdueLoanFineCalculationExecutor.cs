using System;

namespace Specifications.Support
{
    public class OverdueLoanFineCalculationExecutor
    {
        private DateTime _loanDueDate;
        public void SetLoanOverdueContext(int days)
        {
            _loanDueDate = DateTime.Today.AddDays(-days);
        }

        public decimal CalculateOverdueFine()
        {
            return 0;
        }
    }
}
