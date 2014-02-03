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
    using System;
    using NSubstitute;
    using Shouldly;
    using SnuggleBunny.Activity;
    using SnuggleBunny.Budget;
    using SnuggleBunny.Budget.Analyzers;
    using SnuggleBunny.Budget.Config;
    using Xunit;

    public class BudgetToolTests
    {
        public class TheAnalyzeMethod
        {
            [Fact]
            public void RunsAllConfiguredAnalyzers()
            {
                var tool = new BudgetTool();
                var a1 = Substitute.For<ISpendingAnalyzer>();
                var a2 = Substitute.For<ISpendingAnalyzer>();

                tool.AddAnalyzer(a1);
                tool.AddAnalyzer(a2);

                var report = new ActivityReport();
                tool.Analyze(report);

                a1.Received().Initialize(Arg.Any<BudgetConfig>());
                a2.Received().Initialize(Arg.Any<BudgetConfig>());

                a1.Received().Analyze(report);
                a2.Received().Analyze(report);
            }

            [Fact]
            public void DetectsOverspendingInACategory()
            {
                var tool = new BudgetTool();
                tool.Configure(c => { c.Category("clothing").LimitTo(200M); });

                tool.AddAnalyzer(new CategorySpendingLimitAnalyzer());
                var spendingReport = new ActivityReport();
                spendingReport.AddTransaction(new DateTime(2014, 2, 2), "Gap", 201M, "clothing");

                var alerts = tool.Analyze(spendingReport);
                alerts.ShouldContain(new CategorySpendingExceededAlert("clothing", 2, 201M, 200M));
            }

            [Fact]
            public void DetectsMonthsWhereSpendingExceedsMonthlyIncome()
            {
                var tool = new BudgetTool();
                tool.Configure(b => { b.IncomePerMonth(500M); });

                tool.AddAnalyzer(new MonthlySpendingVersusIncomeAnalyzer());

                var spendingReport = new ActivityReport();
                spendingReport.AddTransaction(new DateTime(2014, 1, 2), "Walmart", 499M, "Grocery");
                spendingReport.AddTransaction(new DateTime(2014, 2, 2), "Walmart", 501M, "Grocery");

                var alerts = tool.Analyze(spendingReport);

                alerts.ShouldContain(new MonthlySpendingExceededIncomeAlert(2, 501M, 500M));
            }
        }
    }
}