using System;
using MediaLoanLibrary.Fines.DomainModel.Calculator;

namespace MediaLoanLibrary.Fines.Specifications.Support
{
    public class OverdueLoanFineCalculationExecutor
    {
        private DateTime _loanDueDate;
        private decimal _replacementValue;

        public void SetLoanOverdueContext(int days, decimal replacementValue)
        {
            _loanDueDate = DateTime.Today.AddDays(-days);
            _replacementValue = replacementValue;
        }

        public decimal CalculateOverdueFine()
        {
            var calculator = new FineCalculator();
            return calculator.CalculateOverdueFine((DateTime.Today - _loanDueDate).Days, _replacementValue);
        }
    }
}
