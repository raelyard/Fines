using System;
using MediaLoanLibrary.Fines.DomainModel.Messages.Commands;
using MediaLoanLibrary.Fines.PublicEvents;
using NUnit.Framework;
using Should;

namespace MediaLoanLibrary.Fines.Tests.Common.Bus
{
    public abstract class WhenUsingUnobtrisiveMessageConventionsToIdentifyMessagesBase
    {
        protected abstract bool ExecuteMessagesDefinitionOnType(Type type);

        [Test]
        public void ShouldNotMatchOnPublicEventTypeNotEndingWithEvent()
        {
            ExecuteMessagesDefinitionOnType(typeof(SomethingHappendEventDude)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnInternalFinesEventTypeNotEndingWithEvent()
        {
            ExecuteMessagesDefinitionOnType(typeof(DomainModel.Messages.Events.SomethingHappendEventDude)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnCommandNotEndingWithCommand()
        {
            ExecuteMessagesDefinitionOnType(typeof(DoSomethingCommandDude)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnEventOutsideEventNamespace()
        {
            ExecuteMessagesDefinitionOnType(typeof(DomainModel.SomethingHappendEvent)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnCommandOutsideCommandNamespace()
        {
            ExecuteMessagesDefinitionOnType(typeof(DomainModel.DoSomethingCommand)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnEventWithoutNamespace()
        {
            ExecuteMessagesDefinitionOnType(typeof(SomethingWithoutNamespaceHappendEvent)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnCommandWithoutNamespace()
        {
            ExecuteMessagesDefinitionOnType(typeof(DoSomethingWithoutNamespaceCommand)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnEventFromSomeWildlyOtherNamespace()
        {
            ExecuteMessagesDefinitionOnType(typeof(Twitter.Fines.PublicEvents.SomethingHappendEvent)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnCommandFromSomeWildlyOtherNamespace()
        {
            ExecuteMessagesDefinitionOnType(typeof(Twitter.Fines.DomainModel.Messages.Commands.DoSomethingCommand)).ShouldBeFalse();
        }
    }
}
