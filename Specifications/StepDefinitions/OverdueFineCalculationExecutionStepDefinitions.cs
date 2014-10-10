using MediaLoanLIbrary.Fines.Specifications.Support;
using TechTalk.SpecFlow;

namespace MediaLoanLIbrary.Fines.Specifications.StepDefinitions
{
    [Binding]
    public class OverdueFineCalculationExecutionStepDefinitions
    {
        private readonly OverdueFineCalculationExecutionPolicyManager _manager;

        public OverdueFineCalculationExecutionStepDefinitions(OverdueFineCalculationExecutionPolicyManager manager)
        {
            _manager = manager;
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
    }
}
