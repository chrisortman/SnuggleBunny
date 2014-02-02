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
            var spendingByMonth =
                from transaction in activityReport.Transactions()
                group transaction by
                    new
                    {
                        Year = transaction.OccurredOn.Year,
                        Month = transaction.OccurredOn.Month,
                    }
                into byMonth
                select new
                {
                    Group = byMonth.Key,
                    Total = byMonth.Sum(x => x.Amount)
                };

            var exceeded =
                from spent in spendingByMonth
                where spent.Total > _monthlyIncome
                select new MonthlyIncomeExceededAlert(spent.Group.Month, spent.Total, _monthlyIncome);

            return exceeded;
        }
    }
}