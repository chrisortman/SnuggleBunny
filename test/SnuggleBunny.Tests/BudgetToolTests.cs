using System;
using Shouldly;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget;
using Xunit;

namespace SnuggleBunny.Tests
{
    public class BudgetToolTests
    {
        public class TheAnalyzeMethod
        {
            [Fact]
            public void DetectsOverspendingInACategory()
            {
                var tool = new BudgetTool();
                tool.DefineCategory("clothing", limit: 200M);

                var spendingReport = new ActivityReport();
                spendingReport.AddTransaction(new DateTime(2014, 2, 2), "Gap", 201M, "clothing");

                var alerts = tool.Analyze(spendingReport);
                alerts.ShouldContain(new CategorySpendingExceededAlert("clothing",2,201M,200M));

            }

            [Fact]
            public void DetectsMonthsWhereSpendingExceedsMonthlyIncome()
            {
                var tool = new BudgetTool();
                tool.MonthlyIncome(500M);

                var spendingReport = new ActivityReport();
                spendingReport.AddTransaction(new DateTime(2014,1,2),"Walmart",499M,"Grocery");
                spendingReport.AddTransaction(new DateTime(2014,2,2),"Walmart",501M,"Grocery");

                var alerts = tool.Analyze(spendingReport);

                alerts.ShouldContain(new MonthlyIncomeExceededAlert(2,501M,500M));
            }
        }
    }
}