using System;
using MediaLoanLIbrary.Fines.PublicEvents;
using NUnit.Framework;
using Should;

namespace MediaLoanLIbrary.Fines.Tests.Common.Bus
{
    [TestFixture]
    public class WhenUsingUnobtrisiveMessageConventionsToIdentifyEvents
    {
        [Test]
        public void ShouldMatchOnPublicEventType()
        {
            UnobtrusiveMessageConventions.EventsDefinition(typeof(SomethingHappendEvent)).ShouldBeTrue();
        }

        [Test]
        public void ShouldNotMatchOnPublicEventTypeNotEndingWithEvent()
        {
            UnobtrusiveMessageConventions.EventsDefinition(typeof(SomethingHappendEventDude)).ShouldBeFalse();
        }
    }

    public class UnobtrusiveMessageConventions
    {
        public static Func<Type, bool> EventsDefinition
        {
            get { return type => type.Name.EndsWith("Event"); }
        }
    }
}

namespace MediaLoanLIbrary.Fines.PublicEvents
{
    public interface SomethingHappendEvent
    {
    }

    public interface SomethingHappendEventDude
    {
    }
}
