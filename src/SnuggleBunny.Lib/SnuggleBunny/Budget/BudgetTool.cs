using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget.Config;
using SnuggleBunny.Budget.Config.Builders;

namespace SnuggleBunny.Budget
{
    public class BudgetTool
    {
        private readonly BudgetConfig _config;
        private readonly List<ISpendingAnalyzer> _analyzers;

        public BudgetTool(string configFile)
        {
            Guard.AgainstNull(configFile,"configFile");
            Guard.Against<ArgumentException>(configFile.IsBlank(),"configFile should not be blank");

            var loader = new BudgetConfigLoader();
            _config = loader.LoadFile(configFile);
            _analyzers = new List<ISpendingAnalyzer>();

        }

        public BudgetTool()
        {
            _config = new BudgetConfig();
            _analyzers = new List<ISpendingAnalyzer>();
        }

        public void Configure(Action<BudgetConfigBuilder> action)
        {
            Guard.AgainstNull(action,"action");

            var builder = new BudgetConfigBuilder(_config);
            action(builder);
        }

        public void AddAnalyzer(ISpendingAnalyzer analyzer)
        {
            Guard.AgainstNull(analyzer,"analyzer");
            _analyzers.Add(analyzer);
        }

        public IReadOnlyCollection<ISpendingAlert> Analyze(ActivityReport activityReport)
        {
            Guard.AgainstNull(activityReport,"activityReport");

            _analyzers.ForEach(x => x.Initialize(_config));
            return new ReadOnlyCollection<ISpendingAlert>(
                _analyzers.SelectMany(x => x.Analyze(activityReport)).ToList());
        }
    }
}