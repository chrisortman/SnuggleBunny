using System.Collections.Generic;
using System.Linq;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget.Config;

namespace SnuggleBunny.Budget.Analyzers
{
    public class OverspendInCategoryForMonthAnalyzer : ISpendingAnalyzer
    {
        private List<SpendingCategory> _categories;

        public void Initialize(BudgetConfig config)
        {
            _categories = config.Categories.Values.ToList();
        }

        public IEnumerable<ISpendingAlert> Analyze(ActivityReport activityReport)
        {
            var spendingByCategoryMonth =
                from transaction in activityReport.AllTransactions()
                group transaction by
                    new
                    {
                        Year = transaction.OccurredOn.Year,
                        Month = transaction.OccurredOn.Month,
                        Category = transaction.Category
                    }
                into byMonth
                select new
                {
                    Group = byMonth.Key,
                    Total = byMonth.Sum(x => x.Amount)
                };



            var overages = from spent in spendingByCategoryMonth
                join cat in _categories on spent.Group.Category equals cat.Name
                where spent.Total > cat.Limit
                select new CategorySpendingExceededAlert(cat.Name, spent.Group.Month, spent.Total, cat.Limit);

            return overages.Cast<ISpendingAlert>();
        }
    }
}