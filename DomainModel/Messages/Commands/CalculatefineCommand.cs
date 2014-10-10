using System;

namespace MediaLoanLibrary.Fines.DomainModel.Messages.Commands
{
    public class CalculateFineCommand
    {
        public Guid LoanId { get; set; }
        public int DaysOverdue { get; set; }
    }
}
