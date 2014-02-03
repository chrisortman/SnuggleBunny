using System.Collections.Generic;
using System.Linq;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget.Config;

namespace SnuggleBunny.Budget.Analyzers
{
    public class CategorySpendingLimitAnalyzer : ISpendingAnalyzer
    {
        private Dictionary<string, SpendingCategory> _categories;

        public void Initialize(BudgetConfig config)
        {
            _categories = config.Categories;
        }

        public IEnumerable<ISpendingAlert> Analyze(ActivityReport activityReport)
        {

            var alerts = activityReport.GroupTransactions(By.MonthAndCategory)
                                       .Select(Transactions.Totalled)
                                       .Where(SpentExceedsCategoryLimit)
                                       .Select(SpendingExceededAlert);
            return alerts;
        }

        private SpendingCategory GetCategory(Transactions.TotalledTransactionGroup<By.MonthCategoryGroup> arg)
        {
            Guard.AgainstNull(arg, "arg");
            Guard.Requires(_categories.ContainsKey(arg.Group.Category), "Invalid category");

            var category = _categories[arg.Group.Category];
            return category;
        }

        private bool SpentExceedsCategoryLimit(Transactions.TotalledTransactionGroup<By.MonthCategoryGroup> transactionGroup)
        {
            var category = GetCategory(transactionGroup);
            return transactionGroup.Total > category.Limit;
        }

        private CategorySpendingExceededAlert SpendingExceededAlert(Transactions.TotalledTransactionGroup<By.MonthCategoryGroup> transactionGroup)
        {
            var category = GetCategory(transactionGroup);

            return new CategorySpendingExceededAlert(transactionGroup.Group.Category, 
                                                     transactionGroup.Group.Month, 
                                                     transactionGroup.Total, 
                                                     category.Limit);
        }
    }
}