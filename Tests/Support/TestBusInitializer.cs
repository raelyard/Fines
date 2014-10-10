using MediaLoanLIbrary.Fines.Common.Bus;
using NServiceBus.Testing;

namespace MediaLoanLIbrary.Fines.Tests.Support
{
    public class TestBusInitializer
    {
        public static void Initialize()
        {
            Test.Initialize(configuration =>
            {
                configuration.Conventions().DefiningEventsAs(UnobtrusiveMessageConventions.EventsDefinition);
                configuration.Conventions().DefiningCommandsAs(UnobtrusiveMessageConventions.CommandsDefinition);
            });
            Test.Initialize();
        }
    }
}
