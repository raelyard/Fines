using System.Configuration;
using System.Security.Policy;
using MediaLoanLIbrary.Fines.Common.Bus;
using MediaLoanLibrary.Fines.Specifications.Support.Integration;
using MediaLoanLibrary.Fines.Specifications.Support.Model;
using NServiceBus;
using NServiceBus.Features;

namespace MediaLoanLibrary.Fines.Specifications.Support
{
    public interface OverdueFineCalculationExecutionPolicyManager
    {
        OverdueFineCalculationExecutionPolicyManager EstablishLoan();
        OverdueFineCalculationExecutionPolicyManager EstablishActionContext();
        OverdueFineCalculationExecutionPolicyManager ExpectDailyCalculation();
        OverdueFineCalculationExecutionPolicyManager ExpectDailyCalculationToStop();
    }

    public class OverdueFineCalculationExecutionTestContext
    {
        public OverdueFineCalculationExecutionPolicyManager Manager { get; private set; }

        public OverdueFineCalculationExecutionTestContext()
        {
            switch (ConfigurationManager.AppSettings["ExecuteSpecificationsLevel"])
            {
                case "Integration":
                    Manager = new MessageHandlingOverdueFineCalculationExecutionPolicyManager(ConfigureNServiceBus());
                    break;
                default:
                    Manager = new NServiceBusUnitTestingOverdueFineCalculationExecutionPolicyManager();
                    break;
            }
        }

        private IBus ConfigureNServiceBus()
        {
            var configuration = new BusConfiguration();
            configuration.AssembliesToScan(AllAssemblies.Except("MediaLoanLIbrary.Fines.Common"));
            configuration.EndpointName("IntegrationTesting");
            configuration.DisableFeature<Sagas>();
            configuration.DisableFeature<TimeoutManager>();
            configuration.DisableFeature<AutoSubscribe>();
            configuration.DisableFeature<InMemorySubscriptionPersistence>();
            configuration.DisableFeature<StorageDrivenPublishing>();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.PurgeOnStartup(true);
            configuration.Conventions()
                .DefiningCommandsAs(type => UnobtrusiveMessageConventions.CommandsDefinition(type) || (UnobtrusiveMessageConventions.EventsDefinition(type) && type.Namespace.Contains(".Loans.")))
                .DefiningEventsAs(type => UnobtrusiveMessageConventions.EventsDefinition(type) && !type.Namespace.Contains(".Loans."));
            configuration.EnableInstallers();
            return Bus.Create(configuration).Start();
        }
    }
}
