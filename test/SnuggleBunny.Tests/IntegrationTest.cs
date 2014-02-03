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

namespace SnuggleBunny.Tests
{
    using Shouldly;
    using SnuggleBunny.Activity;
    using SnuggleBunny.Budget;
    using SnuggleBunny.Budget.Analyzers;
    using Xunit;

    public class IntegrationTests
    {
        [Fact]
        public void DetectsOverspendingInACategory()
        {
            var tool = new BudgetTool(@"TestData\budget_config.yml");
            tool.AddAnalyzer(new CategorySpendingLimitAnalyzer());
            tool.AddAnalyzer(new MonthlySpendingVersusIncomeAnalyzer());
            var spendingReport = new ActivityReport();
            spendingReport.Load(@"TestData\transactions.csv");

            var alerts = tool.Analyze(spendingReport);

            alerts.ShouldContain(
                x =>
                    x.ToString() ==
                    "[OVERAGE] - Spending for clothing in Feb exceeded budget by $150.00 ($200.00/$350.00)");
        }
    }
}