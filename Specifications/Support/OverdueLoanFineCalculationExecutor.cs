using System;

namespace Specifications.Support
{
    public class OverdueLoanFineCalculationExecutor
    {
        private DateTime _loanDueDate;
        public void SetLoanOverdueContext()
        {
            _loanDueDate = DateTime.Today;
        }

        public decimal CalculateOverdueFine()
        {
            return 0;
        }
    }
}
