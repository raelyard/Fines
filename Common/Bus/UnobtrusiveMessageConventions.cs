using System;
using NServiceBus;

namespace MediaLoanLIbrary.Fines.Common.Bus
{
    public class UnobtrusiveMessageConventions : INeedInitialization
    {
        public static Func<Type, bool> EventsDefinition
        {
            get
            {
                return type =>
                    type.Namespace != null &&
                    type.Namespace.StartsWith("MediaLoanLibrary.") &&
                    (
                        type.Namespace.Contains(".PublicEvents") ||
                        type.Namespace.Contains(".Events")
                        ) &&
                    type.Name.EndsWith("Event");
            }
        }

        public static Func<Type, bool> CommandsDefinition
        {
            get
            {
                return type =>
                    type.Namespace != null &&
                    (type.Namespace.StartsWith("MediaLoanLibrary.") &&
                     type.Namespace.Contains(".Commands") &&
                     type.Name.EndsWith("Command"));
            }
        }

        public void Customize(BusConfiguration configuration)
        {
            configuration.Conventions().DefiningEventsAs(EventsDefinition).DefiningCommandsAs(CommandsDefinition);
        }
    }
}
