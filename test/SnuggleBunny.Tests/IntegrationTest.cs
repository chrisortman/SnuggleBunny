using System.Collections;
using System.Security.Cryptography;
using Shouldly;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget;
using Xunit;

namespace SnuggleBunny.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void DetectsOverespendingInACategory()
        {
            var tool = new BudgetTool(@"TestData\budget_config.yml");
            var spendingReport = new ActivityReport();
            spendingReport.Load(@"TestData\transactions.csv");

            var alerts = tool.Analyze(spendingReport);

            alerts.ShouldContain(
                x => x.ToString() == "[OVERAGE] - Spending for clothing in Feb exceeded budget by $150.00 ($200.00/$350.00)");

        }
    }
}