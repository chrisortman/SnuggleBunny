using System.Collections.Generic;
using System.Linq;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget.Config;

namespace SnuggleBunny.Budget.Analyzers
{
    public class MonthlySpendingVersusIncomeAnalyzer : ISpendingAnalyzer
    {
        private decimal _monthlyIncome;

        public void Initialize(BudgetConfig config)
        {
            _monthlyIncome = config.MonthlyIncome;
        }

        public IEnumerable<ISpendingAlert> Analyze(ActivityReport activityReport)
        {
            var alerts = activityReport.GroupTransactions(By.Month)
                                       .Select(Transactions.Totalled)
                                       .Where(SpentExceedsIncome)
                                       .Select(IncomeExceededAlert);

            return alerts;                          
              
        }

        private MonthlySpendingExceededIncomeAlert IncomeExceededAlert(TotalledTransactionGroup<MonthGroup> spent)
        {
            return new MonthlySpendingExceededIncomeAlert(spent.Group.Month, spent.Total, _monthlyIncome);
        }


        private bool SpentExceedsIncome(TotalledTransactionGroup<MonthGroup> arg)
        {
            return arg.Total > _monthlyIncome;
        }
    }

   
}