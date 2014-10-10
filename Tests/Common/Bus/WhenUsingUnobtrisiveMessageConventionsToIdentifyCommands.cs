using System;
using MediaLoanLIbrary.Fines.Common.Bus;
using MediaLoanLibrary.Fines.DomainModel.Messages.Commands;
using MediaLoanLibrary.Fines.PublicEvents;
using NUnit.Framework;
using Should;

namespace MediaLoanLibrary.Fines.Tests.Common.Bus
{
    [TestFixture]
    public class WhenUsingUnobtrisiveMessageConventionsToIdentifyCommands : WhenUsingUnobtrisiveMessageConventionsToIdentifyMessagesBase
    {
        [Test]
        public void ShouldMatchOnCommandType()
        {
            ExecuteCommandsDefinitionOnType(typeof(DoSomethingCommand)).ShouldBeTrue();
        }

        [Test]
        public void ShouldNotMatchOnPublicEventType()
        {
            ExecuteCommandsDefinitionOnType(typeof(SomethingHappendEvent)).ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchOnInternalFinesEventType()
        {
            ExecuteCommandsDefinitionOnType(typeof(DomainModel.Messages.Events.SomethingHappendEvent)).ShouldBeFalse();
        }

        protected override bool ExecuteMessagesDefinitionOnType(Type type)
        {
            return ExecuteCommandsDefinitionOnType(type);
        }

        private bool ExecuteCommandsDefinitionOnType(Type type)
        {
            return UnobtrusiveMessageConventions.CommandsDefinition(type);
        } 
    }
}
