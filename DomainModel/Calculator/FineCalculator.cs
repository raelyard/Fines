using System;

namespace MediaLoanLibrary.Fines.DomainModel.Calculator
{
    public class FineCalculator
    {
        private const int GracePeriodDays = 2;
        private const decimal FinePerDay = 0.25m;
        private const decimal ReplacementTimeframeDays = 30;

        public decimal CalculateOverdueFine(int daysOverdue, decimal replacementValue)
        {
            if (daysOverdue <= GracePeriodDays)
            {
                return 0;
            }
            if (daysOverdue >= ReplacementTimeframeDays)
            {
                return replacementValue;
            }
            return Math.Min(daysOverdue * FinePerDay, replacementValue);
        } 
    }
}
