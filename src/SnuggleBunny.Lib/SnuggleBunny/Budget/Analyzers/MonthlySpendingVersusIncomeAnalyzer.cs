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