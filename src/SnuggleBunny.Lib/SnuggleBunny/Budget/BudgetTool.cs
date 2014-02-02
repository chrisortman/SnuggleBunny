using System.Collections.Generic;
using System.Linq;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget.Analyzers;
using SnuggleBunny.Budget.Config;

namespace SnuggleBunny.Budget
{
    public class BudgetTool
    {
        private BudgetConfig _config = new BudgetConfig();
       

        public BudgetTool(string configFile)
        {
            
        }

        public BudgetTool()
        {
            
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

        public void DefineCategory(string clothing, decimal limit)
        {
            _config.DefineCategory(clothing,limit);
        }

        public void MonthlyIncome(decimal amount)
        {
            _config.IncomePerMonth(amount);
        }
    }
}