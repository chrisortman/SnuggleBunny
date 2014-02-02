using System;
using System.Collections.Generic;
using System.Linq;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget.Analyzers;
using SnuggleBunny.Budget.Config;
using SnuggleBunny.Budget.Config.Builders;

namespace SnuggleBunny.Budget
{
    public class BudgetTool
    {
        private readonly BudgetConfig _config;

        public BudgetTool(string configFile)
        {
            var loader = new BudgetConfigLoader();
            _config = loader.LoadFile(configFile);
        }

        public BudgetTool()
        {
            _config = new BudgetConfig();
        }

        public IEnumerable<ISpendingAlert> Analyze(ActivityReport activityReport)
        {
            var analyzers = new List<ISpendingAnalyzer>
            {
                new OverspendInCategoryForMonthAnalyzer(),
                new MonthlySpendingVersusIncomeAnalyzer(),
            };

            analyzers.ForEach(x => x.Initialize(_config));

            return analyzers.SelectMany(x => x.Analyze(activityReport));

        }

        public void Configure(Action<BudgetConfigBuilder> action)
        {
            var builder = new BudgetConfigBuilder(_config);
            action(builder);
        }
    }
}