using System;
using System.Collections.Generic;
using MediaLoanLibrary.Fines.PublicEvents;
using NServiceBus;

namespace MediaLoanLibrary.Fines.Specifications.Support.Integration
{
    public class SpecificationsFineCalculatedEventHandler : IHandleMessages<FineCalculatedEvent>
    {
        public static List<FineCalculatedEvent> FineCalculatedEvents { get; set; }

        static SpecificationsFineCalculatedEventHandler()
        {
            FineCalculatedEvents = new List<FineCalculatedEvent>();
        }

        public void Handle(FineCalculatedEvent message)
        {
            Console.WriteLine("Got an event!");
            FineCalculatedEvents.Add(message);
        }
    }
}
