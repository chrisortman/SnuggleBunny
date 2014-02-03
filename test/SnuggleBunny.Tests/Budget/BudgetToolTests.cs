using System;
using System.Linq;
using NSubstitute;
using Shouldly;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget;
using SnuggleBunny.Budget.Config;
using Xunit;

namespace SnuggleBunny.Tests
{
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
                tool.Configure(c =>
                {
                    c.Category("clothing").LimitTo(200M);
                });

                var spendingReport = new ActivityReport();
                spendingReport.AddTransaction(new DateTime(2014, 2, 2), "Gap", 201M, "clothing");

                var alerts = tool.Analyze(spendingReport);
                alerts.ShouldContain(new CategorySpendingExceededAlert("clothing",2,201M,200M));

            }

            [Fact]
            public void DetectsMonthsWhereSpendingExceedsMonthlyIncome()
            {
                var tool = new BudgetTool();
                tool.Configure(b =>
                {
                    b.IncomePerMonth(500M);
                });

                var spendingReport = new ActivityReport();
                spendingReport.AddTransaction(new DateTime(2014,1,2),"Walmart",499M,"Grocery");
                spendingReport.AddTransaction(new DateTime(2014,2,2),"Walmart",501M,"Grocery");

                var alerts = tool.Analyze(spendingReport);

                alerts.ShouldContain(new MonthlyIncomeExceededAlert(2,501M,500M));
            }
        }
    }
}