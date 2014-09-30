using Should;
using Specifications.Support;
using TechTalk.SpecFlow;

namespace Specifications.StepDefinitions
{
    [Binding]
    public class OverdueFineCalculationStepDefinitions
    {
        private const decimal TypeicalReplacementValue = 9.95m;

        private readonly OverdueLoanFineCalculationExecutor _overdueLoanFineLoanFineCalculationExecutor;
        private decimal _calculatedFine;

        public OverdueFineCalculationStepDefinitions(OverdueLoanFineCalculationExecutor overdueLoanFineLoanFineCalculationExecutor)
        {
            _overdueLoanFineLoanFineCalculationExecutor = overdueLoanFineLoanFineCalculationExecutor;
        }

        [Given(@"A loan is overdue by (.*) days")]
        public void GivenALoanIsOverdueByDays(int days)
        {
            _overdueLoanFineLoanFineCalculationExecutor.SetLoanOverdueContext(days, TypeicalReplacementValue);
        }

        [Given(@"A loan for an item with replacement value \$(.*) is overdue by (.*) days")]
        public void GivenALoanForAnItemWithReplacementValueIsOverdueByDays(decimal replacementValue, int days)
        {
            _overdueLoanFineLoanFineCalculationExecutor.SetLoanOverdueContext(days, replacementValue);
        }

        [When(@"I calculate the overdue fine")]
        public void WhenICalculateTheOverdueFine()
        {
            _calculatedFine = _overdueLoanFineLoanFineCalculationExecutor.CalculateOverdueFine();
        }

        [Then(@"I should see a fine of \$(.*)")]
        public void ThenIShouldSeeAFineOf(decimal expectedFine)
        {
            _calculatedFine.ShouldEqual(expectedFine);
        }
    }
}
