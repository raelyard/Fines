using System;
using MediaLoanLibrary.Fines.DomainModel.Messages.Commands;
using MediaLoanLibrary.Loans.PublicEvents;
using NServiceBus.Saga;

namespace MediaLoanLIbrary.Fines.Specifications.Support
{
    public class OverdueFineAccumulationPolicy : Saga<OverdueFineAccumulationPolicyState>, IAmStartedByMessages<LoanConsumatedEvent>, IHandleTimeouts<FineAccumulationIncrementTimeout>
    {
        private const int DefaultLoanTermDays = 21;
        private const int GracePeriodDays = 2;
        private const int FirstOverdueCalculationDaysOverdue = GracePeriodDays + 1;
        private const int ReplacementTimeframeDays = 30;

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OverdueFineAccumulationPolicyState> mapper)
        {
            mapper.ConfigureMapping<LoanConsumatedEvent>(theEvent => theEvent.LoanId).ToSaga(saga => saga.LoanId);
        }

        public void Handle(LoanConsumatedEvent theEvent)
        {
            Data.LoanId = theEvent.LoanId;
            RequestTimeout<FineAccumulationIncrementTimeout>(DateTime.Today.AddDays(DefaultLoanTermDays + FirstOverdueCalculationDaysOverdue), timeout => timeout.DaysOverdue = FirstOverdueCalculationDaysOverdue);
        }

        public void Timeout(FineAccumulationIncrementTimeout state)
        {
            CalcualteCurrentFine(state.DaysOverdue);
        }

        private void CalcualteCurrentFine(int daysOverdue)
        {
            Bus.Send<CalculateFineCommand>(command =>
            {
                command.LoanId = Data.LoanId;
                command.DaysOverdue = daysOverdue;
            });
            if (daysOverdue >= ReplacementTimeframeDays)
            {
                MarkAsComplete();
                return;
            }
            RequestTimeout<FineAccumulationIncrementTimeout>(TimeSpan.FromDays(1), timeout => timeout.DaysOverdue = daysOverdue + 1);
        }
    }

    public class OverdueFineAccumulationPolicyState : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public Guid LoanId { get; set; }
    }

    public class FineAccumulationIncrementTimeout
    {
        public int DaysOverdue { get; set; }
    }
}
