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
        public void ShouldNotMatchOnEventwithoutNamespace()
        {
            ExecuteEventsDefinitionOnType(typeof(SomethingWithoutNamespaceHappendEvent)).ShouldBeFalse();
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
                    type.Namespace.Contains(".PublicEvents") &&
                    type.Name.EndsWith("Event");
            }
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

namespace MediaLoanLIbrary.Fines.DomainModel
{
    public interface SomethingHappendEvent
    {
    }
}

public interface SomethingWithoutNamespaceHappendEvent
{
}