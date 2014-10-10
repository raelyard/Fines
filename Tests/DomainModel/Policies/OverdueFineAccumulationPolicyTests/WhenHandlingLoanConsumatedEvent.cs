using System;
using MediaLoanLIbrary.Fines.Specifications.Support;
using MediaLoanLIbrary.Fines.Tests.Support;
using MediaLoanLibrary.Loans.PublicEvents;
using NServiceBus.Testing;
using NUnit.Framework;
using Should;

namespace MediaLoanLIbrary.Fines.Tests.DomainModel.Policies.OverdueFineAccumulationPolicyTests
{
    [TestFixture]
    public class WhenHandlingLoanConsumatedEvent
    {
        private readonly Guid _loanId = Guid.NewGuid();

        [Test]
        public void ShouldSetLoanIdOnState()
        {
            TestBusInitializer.Initialize();
            var saga = new OverdueFineAccumulationPolicy{};
            Test.Saga(saga);
            saga.Handle(Test.CreateInstance<LoanConsumatedEvent>(theEvent => theEvent.LoanId = _loanId));
            saga.Data.LoanId.ShouldEqual(_loanId);
        }
    }
}
