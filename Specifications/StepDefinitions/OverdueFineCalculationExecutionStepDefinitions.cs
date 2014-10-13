using System;
using MediaLoanLibrary.Fines.Specifications.Support;
using TechTalk.SpecFlow;

namespace MediaLoanLibrary.Fines.Specifications.StepDefinitions
{
    [Binding]
    public class OverdueFineCalculationExecutionStepDefinitions
    {
        private readonly OverdueFineCalculationExecutionPolicyManager _manager;

        public OverdueFineCalculationExecutionStepDefinitions(OverdueFineCalculationExecutionTestContext context)
        {
            _manager = context.Manager;
        }

        [Given(@"A loan has been established")]
        public void GivenALoanHasBeenEstablished()
        {
            _manager.EstablishLoan();
        }

        [When(@"The third day following the due date has arrived")]
        public void WhenTheThirdDayFollowingTheDueDateHasArrived()
        {
            _manager.EstablishActionContext();
        }

        [Then(@"the fine should begin daily calculation")]
        public void ThenTheFineShouldBeginDailyCalculation()
        {
            _manager.ExpectDailyCalculation();
        }

        [Then(@"the daily calculation should stop after the thirtieth day after the due date")]
        public void ThenTheDailyCalculationShouldStopAfterTheThirtiethDayAfterTheDueDate()
        {
            _manager.ExpectDailyCalculationToStop();
        }

        [AfterScenario("ExecutesProcessorInIntegrationMode")]
        public void Cleanup()
        {
            var disposableManager = _manager as IDisposable;
            if (disposableManager != null)
            {
                disposableManager.Dispose();
            }
        }
    }
}
