using System;
using System.Configuration;
using MediaLoanLibrary.Fines.DomainModel.Messages.Commands;
using MediaLoanLibrary.Loans.PublicEvents;
using NServiceBus.Saga;

namespace MediaLoanLibrary.Fines.DomainModel.Policies
{
    public class OverdueFineAccumulationPolicy : Saga<OverdueFineAccumulationPolicyState>, IAmStartedByMessages<LoanConsumatedEvent>, IHandleTimeouts<FineAccumulationIncrementTimeout>
    {
        private const int GracePeriodDays = 2;
        private const int FirstOverdueCalculationDaysOverdue = GracePeriodDays + 1;
        private const int ReplacementTimeframeDays = 30;

        private readonly int _dailyIncrementDays;
        private readonly Func<DateTime, DateTime> _getfirstOverdueCalculationTimeoutDate;

        public OverdueFineAccumulationPolicy()
        {
            var immediateTimeouts = false;
            bool.TryParse(ConfigurationManager.AppSettings["ImmediateTimeouts"], out immediateTimeouts);
            if (immediateTimeouts)
            {
                _dailyIncrementDays = 0;
                _getfirstOverdueCalculationTimeoutDate = dueDate => DateTime.Now;
            }
            else
            {
                _dailyIncrementDays = 1;
                _getfirstOverdueCalculationTimeoutDate = duedate => duedate.AddDays(FirstOverdueCalculationDaysOverdue);
            }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OverdueFineAccumulationPolicyState> mapper)
        {
            mapper.ConfigureMapping<LoanConsumatedEvent>(theEvent => theEvent.LoanId).ToSaga(saga => saga.LoanId);
        }

        public void Handle(LoanConsumatedEvent theEvent)
        {
            Data.LoanId = theEvent.LoanId;
            RequestTimeout<FineAccumulationIncrementTimeout>(_getfirstOverdueCalculationTimeoutDate(theEvent.DueDate), timeout => timeout.DaysOverdue = FirstOverdueCalculationDaysOverdue);
        }

        public void Timeout(FineAccumulationIncrementTimeout state)
        {
            CalculateCurrentFine(state.DaysOverdue);
            RequestNextFineCalculationOrCompletePolicy(state.DaysOverdue);
        }

        private void CalculateCurrentFine(int daysOverdue)
        {
            Bus.Send<CalculateFineCommand>(command =>
            {
                command.LoanId = Data.LoanId;
                command.DaysOverdue = daysOverdue;
            });
        }

        private void RequestNextFineCalculationOrCompletePolicy(int daysOverdue)
        {
            if (daysOverdue >= ReplacementTimeframeDays)
            {
                MarkAsComplete();
                return;
            }
            RequestTimeout<FineAccumulationIncrementTimeout>(TimeSpan.FromDays(_dailyIncrementDays), timeout => timeout.DaysOverdue = daysOverdue + 1);
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
