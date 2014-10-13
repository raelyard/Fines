using Autofac;
using MediaLoanLibrary.Fines.DomainModel.Calculator;
using NServiceBus;
using NServiceBus.Features;

namespace MediaLoanLibrary.Fines.Processor
{
    [EndpointName("Fines")]
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, INeedInitialization
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UseContainer<AutofacBuilder>(customizations => customizations.ExistingLifetimeScope(ConfigureContainer()));

            configuration.UsePersistence<InMemoryPersistence>();
            configuration.EnableFeature<TimeoutManager>();
        }

        private IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FineCalculator>().AsSelf();
            var container = builder.Build();

            return container;
        }
    }
}
