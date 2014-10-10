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
        private const int LoanId = 666;

        [Test]
        public void ShouldSetLoanIdOnState()
        {
            TestBusInitializer.Initialize();
            var saga = new OverdueFineAccumulationPolicy{};
            Test.Saga(saga);
            saga.Handle(Test.CreateInstance<LoanConsumatedEvent>(theEvent => theEvent.LoanId = LoanId));
            saga.Data.LoanId.ShouldEqual(LoanId);
        }
    }
}
