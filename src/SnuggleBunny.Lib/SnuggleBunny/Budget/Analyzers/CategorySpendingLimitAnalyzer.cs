#region LICENSE

// The MIT License (MIT)
// 
// Copyright (c) <year> <copyright holders>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

namespace SnuggleBunny.Budget.Analyzers
{
    using System.Collections.Generic;
    using System.Linq;
    using Activity;
    using Config;

    /// <summary>
    /// Analyzes a spending report looking for instances where
    /// the user has spent more money in a given category than
    /// they alotted in their budget.
    /// </summary>
    public class CategorySpendingLimitAnalyzer : ISpendingAnalyzer
    {
        private Dictionary<string, SpendingCategory> _categories;

        /// <summary>
        /// Initializes this analyzer
        /// </summary>
        /// <param name="config"></param>
        public void Initialize(BudgetConfig config)
        {
            _categories = config.Categories;
        }

        /// <summary>
        /// Analyze the activity report for months where the category
        /// limit was exceeded
        /// </summary>
        /// <param name="activityReport"></param>
        /// <returns></returns>
        public IEnumerable<ISpendingAlert> Analyze(ActivityReport activityReport)
        {
            var alerts = activityReport.GroupTransactions(By.MonthAndCategory)
                .Select(Transactions.Totalled)
                .Where(SpentExceedsCategoryLimit)
                .Select(SpendingExceededAlert);
            return alerts;
        }

        [NotNull]
        private SpendingCategory GetCategory(TotalledTransactionGroup<MonthCategoryGroup> arg)
        {
            Guard.AgainstNull(arg, "arg");
            Guard.Requires(_categories.ContainsKey(arg.Group.Category), "Invalid category");

            var category = _categories[arg.Group.Category];
            Guard.Requires(category != null,"category existed in dictionary but was null");

            return category;
        }

        private bool SpentExceedsCategoryLimit(TotalledTransactionGroup<MonthCategoryGroup> transactionGroup)
        {
            var category = GetCategory(transactionGroup);
            return transactionGroup.Total > category.Limit;
        }

        private CategorySpendingExceededAlert SpendingExceededAlert(
            TotalledTransactionGroup<MonthCategoryGroup> transactionGroup)
        {
            var category = GetCategory(transactionGroup);

            return new CategorySpendingExceededAlert(transactionGroup.Group.Category,
                transactionGroup.Group.Month,
                transactionGroup.Total,
                category.Limit);
        }
    }
}