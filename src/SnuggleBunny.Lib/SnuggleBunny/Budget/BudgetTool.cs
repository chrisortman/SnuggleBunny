using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<ISpendingAnalyzer> _analyzers;

        public BudgetTool(string configFile)
        {
            var loader = new BudgetConfigLoader();
            _config = loader.LoadFile(configFile);
            _analyzers = new List<ISpendingAnalyzer>();
        }

        public BudgetTool()
        {
            _config = new BudgetConfig();
            _analyzers = new List<ISpendingAnalyzer>();
        }

        public IReadOnlyCollection<ISpendingAlert> Analyze(ActivityReport activityReport)
        {
            _analyzers.ForEach(x => x.Initialize(_config));
            return new ReadOnlyCollection<ISpendingAlert>(
                _analyzers.SelectMany(x => x.Analyze(activityReport)).ToList());
        }

        public void Configure(Action<BudgetConfigBuilder> action)
        {
            var builder = new BudgetConfigBuilder(_config);
            action(builder);
        }

        public void AddAnalyzer(ISpendingAnalyzer analyzer)
        {
            _analyzers.Add(analyzer);
        }
    }
}