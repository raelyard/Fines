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
            return CalculateOverdueFine(DateTime.Today);
        }


        private const int GracePeriodDays = 2;
        private const decimal FinePerDay = 0.25m;

        private decimal CalculateOverdueFine(DateTime targetDate)
        {
            var daysOverdue = (targetDate - _loanDueDate).Days;
            if (daysOverdue <= GracePeriodDays)
            {
                return 0;
            }
            return daysOverdue*FinePerDay;
        }
    }
}
