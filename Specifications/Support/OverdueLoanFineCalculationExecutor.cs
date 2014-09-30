using System;

namespace Specifications.Support
{
    public class OverdueLoanFineCalculationExecutor
    {
        public void SetLoanOverdueContext(int days, decimal replacementValue)
        {
            _loanDueDate = DateTime.Today.AddDays(-days);
            _replacementValue = replacementValue;
        }

        public decimal CalculateOverdueFine()
        {
            return CalculateOverdueFine(DateTime.Today);
        }


        private const int GracePeriodDays = 2;
        private const decimal FinePerDay = 0.25m;
        private const decimal ReplacementTimeframeDays = 30;

        private DateTime _loanDueDate;
        private decimal _replacementValue;

        private decimal CalculateOverdueFine(DateTime targetDate)
        {
            var daysOverdue = (targetDate - _loanDueDate).Days;
            if (daysOverdue <= GracePeriodDays)
            {
                return 0;
            }
            if (daysOverdue >= ReplacementTimeframeDays)
            {
                return _replacementValue;
            }
            return Math.Min(daysOverdue * FinePerDay, _replacementValue);
        }
    }
}
