using System;
using MediaLoanLibrary.Fines.DomainModel.Commands;
using MediaLoanLibrary.Fines.PublicEvents;
using NUnit.Framework;
using Should;

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
            ExecuteEventsDefinitionOnType(typeof(DomainModel.Events.SomethingHappendEvent)).ShouldBeTrue();
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
