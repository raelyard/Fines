using System;

namespace MediaLoanLibrary.Fines.Tests.Common.Bus
{
    public class UnobtrusiveMessageConventions
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
    }
}
