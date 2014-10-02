using System;
using MediaLoanLibrary.Fines.PublicEvents;
using NUnit.Framework;
using Should;

namespace MediaLoanLibrary.Fines.Tests.Common.Bus
{
    [TestFixture]
    public class WhenUsingUnobtrisiveMessageConventionsToIdentifyEvents
    {
        [Test]
        public void ShouldMatchOnPublicEventType()
        {
            ExecuteEventsDefinitionOnType(typeof(SomethingHappendEvent)).ShouldBeTrue();
        }

        [Test]
        public void ShouldNotMatchOnPublicEventTypeNotEndingWithEvent()
        {
            ExecuteEventsDefinitionOnType(typeof(SomethingHappendEventDude)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnEventOutsideEventNamespace()
        {
            ExecuteEventsDefinitionOnType(typeof(DomainModel.SomethingHappendEvent)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnEventWithoutNamespace()
        {
            ExecuteEventsDefinitionOnType(typeof(SomethingWithoutNamespaceHappendEvent)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnEventFromSomeWildlyOtherNamespace()
        {
            ExecuteEventsDefinitionOnType(typeof(Twitter.Fines.PublicEvents.SomethingHappendEvent)).ShouldBeFalse();
        }

        private bool ExecuteEventsDefinitionOnType(Type type)
        {
            return UnobtrusiveMessageConventions.EventsDefinition(type);
        }
    }

    public class UnobtrusiveMessageConventions
    {
        public static Func<Type, bool> EventsDefinition
        {
            get
            {
                return type =>
                    type.Namespace != null &&
                    type.Namespace.StartsWith("MediaLoanLibrary.") &&
                    type.Namespace.Contains(".PublicEvents") &&
                    type.Name.EndsWith("Event");
            }
        }
    }
}

namespace MediaLoanLibrary.Fines.PublicEvents
{
    public interface SomethingHappendEvent
    {
    }

    public interface SomethingHappendEventDude
    {
    }
}

namespace Twitter.Fines.PublicEvents
{
    public interface SomethingHappendEvent
    {
    }
}

namespace MediaLoanLibrary.Fines.DomainModel
{
    public interface SomethingHappendEvent
    {
    }
}

public interface SomethingWithoutNamespaceHappendEvent
{
}
