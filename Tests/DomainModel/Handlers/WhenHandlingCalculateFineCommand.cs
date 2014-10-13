using System;
using MediaLoanLibrary.Fines.DomainModel.Calculator;
using MediaLoanLibrary.Fines.DomainModel.Handlers;
using MediaLoanLibrary.Fines.DomainModel.Messages.Commands;
using MediaLoanLibrary.Fines.PublicEvents;
using MediaLoanLIbrary.Fines.Tests.Support;
using NServiceBus.Testing;
using NUnit.Framework;
using Should;

namespace MediaLoanLIbrary.Fines.Tests.DomainModel.Handlers
{
    [TestFixture]
    public class WhenHandlingCalculateFineCommand
    {
        private readonly Guid _loanId = Guid.NewGuid();

        private Handler<CalculateFineCommandHandler> _handler;

        private FineCalculatedEvent _resultingEvent;

        [TestFixtureSetUp]
        public void Setup()
        {

            TestBusInitializer.Initialize();
            _handler = Test.Handler(bus => new CalculateFineCommandHandler(bus, new FineCalculator()));
            _handler
                .ExpectPublish<FineCalculatedEvent>(theEvent =>
                {
                    _resultingEvent = theEvent;
                    return true;
                })
                .OnMessage<CalculateFineCommand>(command =>
                {
                    command.LoanId = _loanId;
                    command.DaysOverdue = 5;
                });
        }

        [Test]
        public void ShouldCallCalculator()
        {
            _resultingEvent.FineAmount.ShouldEqual(1.25m);
        }

        [Test]
        public void ShouldSetLoanId()
        {
            _resultingEvent.LoanId.ShouldEqual(_loanId);
        }
    }
}
