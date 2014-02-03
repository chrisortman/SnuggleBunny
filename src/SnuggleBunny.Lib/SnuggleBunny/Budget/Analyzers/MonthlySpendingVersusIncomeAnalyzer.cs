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
    /// Analyzes a <see cref="ActivityReport"/> for months where the total
    /// amount spent exceeds the monthly income.
    /// </summary>
    public class MonthlySpendingVersusIncomeAnalyzer : ISpendingAnalyzer
    {
        private decimal _monthlyIncome;

        /// <summary>
        /// Initializes this analyzer
        /// </summary>
        /// <param name="config"></param>
        public void Initialize(BudgetConfig config)
        {
            _monthlyIncome = config.MonthlyIncome;
        }

        /// <summary>
        /// Analyzes the activity report for months where the spending
        /// exceeded the available income.
        /// </summary>
        /// <param name="activityReport"></param>
        /// <returns></returns>
        public IEnumerable<ISpendingAlert> Analyze(ActivityReport activityReport)
        {
            Guard.AgainstNull(activityReport,"activityReport");

            var alerts = activityReport.GroupTransactions(By.Month)
                .Select(Transactions.Totalled)
                .Where(SpentExceedsIncome)
                .Select(IncomeExceededAlert);

            return alerts;
        }

        [NotNull]
        private MonthlySpendingExceededIncomeAlert IncomeExceededAlert(TotalledTransactionGroup<MonthGroup> totalledTransactions)
        {
            Guard.AgainstNull(totalledTransactions, "totalledTransactions");

            return new MonthlySpendingExceededIncomeAlert(totalledTransactions.Group.Month, totalledTransactions.Total, _monthlyIncome);
        }


        private bool SpentExceedsIncome(TotalledTransactionGroup<MonthGroup> arg)
        {
            return arg.Total > _monthlyIncome;
        }
    }
}