using MediaLoanLibrary.Fines.DomainModel.Calculator;
using MediaLoanLibrary.Fines.DomainModel.Messages.Commands;
using MediaLoanLibrary.Fines.PublicEvents;
using NServiceBus;

namespace MediaLoanLibrary.Fines.DomainModel.Handlers
{
    public class CalculateFineCommandHandler : IHandleMessages<CalculateFineCommand>
    {
        // TODO: this is probably something that comes from a datastore based on a value passed through in the LoanConsumatedEvent that identifies the media item
        private const decimal ReplacementValue = 9.95m;

        private readonly IBus _bus;
        private readonly FineCalculator _fineCalculator;

        public CalculateFineCommandHandler(IBus bus, FineCalculator fineCalculator)
        {
            _bus = bus;
            _fineCalculator = fineCalculator;
        }

        public void Handle(CalculateFineCommand message)
        {
            var fine = _fineCalculator.CalculateOverdueFine(message.DaysOverdue, ReplacementValue);
            _bus.Publish<FineCalculatedEvent>(theEvent =>
            {
                theEvent.LoanId = message.LoanId;
                theEvent.FineAmount = fine;
            });
        }
    }
}
