using Should;
using Specifications.Support;
using TechTalk.SpecFlow;

namespace Specifications.StepDefinitions
{
    [Binding]
    public class OverdueFineCalculationStepDefinitions
    {
        private readonly OverdueLoanFineCalculationExecutor _overdueLoanFineLoanFineCalculationExecutor;
        private decimal _calculatedFine;

        public OverdueFineCalculationStepDefinitions(OverdueLoanFineCalculationExecutor overdueLoanFineLoanFineCalculationExecutor)
        {
            _overdueLoanFineLoanFineCalculationExecutor = overdueLoanFineLoanFineCalculationExecutor;
        }

        [Given(@"A loan is overdue by 0 days")]
        public void GivenALoanIsOverdueByZeroDays()
        {
            _overdueLoanFineLoanFineCalculationExecutor.SetLoanOverdueContext();
        }

        [When(@"I calculate the overdue fine")]
        public void WhenICalculateTheOverdueFine()
        {
            _calculatedFine = _overdueLoanFineLoanFineCalculationExecutor.CalculateOverdueFine();
        }

        [Then(@"I should see a fine of \$0.00")]
        public void ThenIShouldSeeAFineOf()
        {
            _calculatedFine.ShouldEqual(0);
        }
    }
}
