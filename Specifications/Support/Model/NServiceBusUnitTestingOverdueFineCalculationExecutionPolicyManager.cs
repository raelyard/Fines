using System;
using MediaLoanLibrary.Fines.DomainModel.Messages.Commands;
using MediaLoanLibrary.Fines.DomainModel.Policies;
using MediaLoanLibrary.Loans.PublicEvents;
using NServiceBus.Testing;

namespace MediaLoanLibrary.Fines.Specifications.Support.Model
{
    public class NServiceBusUnitTestingOverdueFineCalculationExecutionPolicyManager : OverdueFineCalculationExecutionPolicyManager
    {
        private const int DefaultLoanTermDays = 21;
        private const int GracePeriodDays = 2;

        private readonly Guid _loanId = Guid.NewGuid();

        private Saga<OverdueFineAccumulationPolicy> _loanDuePolicySagaTester;
        private DateTime _dueDate;

        public OverdueFineCalculationExecutionPolicyManager EstablishLoan()
        {
            TestBusInitializer.Initialize();
            _dueDate = DateTime.Today.AddDays(DefaultLoanTermDays);
            _loanDuePolicySagaTester = Test.Saga<OverdueFineAccumulationPolicy>();
            return this;
        }

        public OverdueFineCalculationExecutionPolicyManager EstablishActionContext()
        {
            _loanDuePolicySagaTester
                .ExpectTimeoutToBeSetAt<FineAccumulationIncrementTimeout>((timeout, timeoutDate) => timeoutDate == _dueDate.AddDays(GracePeriodDays + 1) && timeout.DaysOverdue == GracePeriodDays + 1)
                .When(saga => saga.Handle(Test.CreateInstance<LoanConsumatedEvent>(newEvent =>
                {
                    newEvent.LoanId = _loanId;
                    newEvent.DueDate = _dueDate;
                })));

            return this;
        }

        public OverdueFineCalculationExecutionPolicyManager ExpectDailyCalculation()
        {
            for (var i = 3; i < 30; ++i)
            {
                _loanDuePolicySagaTester
                    .ExpectSend<CalculateFineCommand>(command => command.LoanId == _loanId && command.DaysOverdue == i)
                    .ExpectTimeoutToBeSetIn<FineAccumulationIncrementTimeout>((timeout, timeoutTimespan) => timeoutTimespan == TimeSpan.FromDays(1) && timeout.DaysOverdue == i + 1)
                    .WhenSagaTimesOut();
            }
            return this;
        }

        public OverdueFineCalculationExecutionPolicyManager ExpectDailyCalculationToStop()
        {
            _loanDuePolicySagaTester
                .WhenSagaTimesOut()
                .AssertSagaCompletionIs(true);
            return this;
        }
    }
}
