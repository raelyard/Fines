using System;
using MediaLoanLIbrary.Fines.Common.Bus;
using MediaLoanLibrary.Fines.DomainModel;
using NUnit.Framework;
using Should;
using SomethingHappendEvent = MediaLoanLibrary.Fines.PublicEvents.SomethingHappendEvent;

namespace MediaLoanLibrary.Fines.Tests.Common.Bus
{
    [TestFixture]
    public class WhenUsingUnobtrisiveMessageConventionsToIdentifyEvents : WhenUsingUnobtrisiveMessageConventionsToIdentifyMessagesBase
    {
        [Test]
        public void ShouldMatchOnPublicEventType()
        {
            ExecuteEventsDefinitionOnType(typeof(SomethingHappendEvent)).ShouldBeTrue();
        }

        [Test]
        public void ShouldMatchOnInternalFinesEventType()
        {
            ExecuteEventsDefinitionOnType(typeof(DomainModel.Messages.Events.SomethingHappendEvent)).ShouldBeTrue();
        }

        [Test]
        public void ShouldNotMatchOnCommand()
        {
            ExecuteEventsDefinitionOnType(typeof(DoSomethingCommand)).ShouldBeFalse();
        }

        protected override bool ExecuteMessagesDefinitionOnType(Type type)
        {
            return ExecuteEventsDefinitionOnType(type);
        }

        private bool ExecuteEventsDefinitionOnType(Type type)
        {
            return UnobtrusiveMessageConventions.EventsDefinition(type);
        }
    }
}
