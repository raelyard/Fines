using Specifications.Support;
using TechTalk.SpecFlow;

namespace Specifications.StepDefinitions
{
    [Binding]
    public class OverdueFineCalculationStepDefinitions
    {
        private readonly OverdueLoanFineCalculationExecutor _overdueLoanFineLoanFineCalculationExecutor;

        public OverdueFineCalculationStepDefinitions(OverdueLoanFineCalculationExecutor overdueLoanFineLoanFineCalculationExecutor)
        {
            _overdueLoanFineLoanFineCalculationExecutor = overdueLoanFineLoanFineCalculationExecutor;
        }

        [Given(@"A loan is overdue by 0 days")]
        public void GivenALoanIsOverdueByZeroDays()
        {
            _overdueLoanFineLoanFineCalculationExecutor.SetLoanOverdueContext();
        }
    }
}
