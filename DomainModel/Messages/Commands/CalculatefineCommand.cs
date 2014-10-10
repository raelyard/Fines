namespace MediaLoanLibrary.Fines.DomainModel.Messages.Commands
{
    public class CalculateFineCommand
    {
        public int LoanId { get; set; }
        public int DaysOverdue { get; set; }
    }
}
