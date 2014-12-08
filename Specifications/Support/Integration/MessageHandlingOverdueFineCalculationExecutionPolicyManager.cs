using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediaLoanLibrary.Fines.PublicEvents;
using MediaLoanLibrary.Loans.PublicEvents;
using NServiceBus;
using Should;

namespace MediaLoanLibrary.Fines.Specifications.Support.Integration
{
    public class MessageHandlingOverdueFineCalculationExecutionPolicyManager: OverdueFineCalculationExecutionPolicyManager, IDisposable
    {
        private readonly Guid LoanId = Guid.NewGuid();

        private readonly Process _finesProcessorHostInstallationProcess;
        private readonly Process _finesProcessorHostProcess;
        private readonly IBus _bus;

        public MessageHandlingOverdueFineCalculationExecutionPolicyManager(IBus bus)
        {
            _bus = bus;
            _finesProcessorHostInstallationProcess = GetHostExecutableProcess("/install");
            _finesProcessorHostProcess = GetHostExecutableProcess();
        }

        public OverdueFineCalculationExecutionPolicyManager EstablishLoan()
        {
            Initialize();

            _bus.Send<LoanConsumatedEvent>(theEvent =>
            {
                theEvent.LoanId = LoanId;
                theEvent.DueDate = DateTime.Today.AddDays(21);
            });
            return this;
        }

        private void Initialize()
        {
            SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.Clear();

            ConfigureApplicationForImmediateTimeouts(true);

            _finesProcessorHostInstallationProcess.Start();
            _bus.Subscribe<FineCalculatedEvent>();
            _finesProcessorHostInstallationProcess.WaitForExit();

            _finesProcessorHostProcess.Start();
        }

        public OverdueFineCalculationExecutionPolicyManager EstablishActionContext()
        {
            var giveUpTime = DateTime.Now.AddSeconds(30);
            while (!SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.Any() && DateTime.Now < giveUpTime)
            {
            }
            var firstEvent = SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.First();
            firstEvent.LoanId.ShouldEqual(LoanId);
            firstEvent.FineAmount.ShouldEqual(0.75m);
            return this;
        }

        public OverdueFineCalculationExecutionPolicyManager ExpectDailyCalculation()
        {
            var giveUpTime = DateTime.Now.AddSeconds(30);
            while (SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.Count < 28 && DateTime.Now < giveUpTime)
            {
            }
            SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.Count(theEvent => theEvent.LoanId == LoanId).ShouldEqual(28);
            SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.Count(theEvent => theEvent.LoanId != LoanId).ShouldEqual(0);
            for (var expectedFine = 1m; expectedFine <= 7.25m; expectedFine = expectedFine + 0.25m)
            {
                SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.Any(theEvent => theEvent.FineAmount == expectedFine).ShouldBeTrue(string.Format("Didn't find an event with a fine of {0}", expectedFine));
            }
            SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.Last().FineAmount.ShouldEqual(9.95m);
            return this;
        }

        public OverdueFineCalculationExecutionPolicyManager ExpectDailyCalculationToStop()
        {
            Task.Delay(2000).Wait();

            SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.Count(theEvent => theEvent.LoanId == LoanId).ShouldEqual(28);
            SpecificationsFineCalculatedEventHandler.FineCalculatedEvents.Count(theEvent => theEvent.LoanId != LoanId).ShouldEqual(0);

            return this;
        }

        private Process GetHostExecutableProcess(string arguments = null)
        {
            return new Process {StartInfo = new ProcessStartInfo(@"..\..\..\Processor\bin\Debug\NServiceBus.Host.exe", arguments)};
        }

        private void ConfigureApplicationForImmediateTimeouts(bool immediate)
        {
            const string applicationConfigFile = @"..\..\..\Processor\bin\Debug\MediaLoanLibrary.Fines.Processor.dll.config";
            var document = XDocument.Load(applicationConfigFile);

            document.Root.Element("appSettings").Elements().First(element => element.Attribute("key").Value == "ImmediateTimeouts").Attribute("value").SetValue(immediate.ToString());

            // Save the new setting
            document.Save(applicationConfigFile);
        }

        public void Dispose()
        {
            ConfigureApplicationForImmediateTimeouts(false);
            if (_finesProcessorHostProcess != null && !_finesProcessorHostProcess.HasExited)
            {
                _finesProcessorHostProcess.CloseMainWindow();
                _finesProcessorHostProcess.Close();
                _finesProcessorHostProcess.Dispose();
            }
        }
    }
}
