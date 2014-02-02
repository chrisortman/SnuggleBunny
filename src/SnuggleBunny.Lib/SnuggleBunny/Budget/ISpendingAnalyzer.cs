using System.Collections.Generic;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget.Config;

namespace SnuggleBunny.Budget
{
    public interface ISpendingAnalyzer
    {
        void Initialize(BudgetConfig config);
        IEnumerable<ISpendingAlert> Analyze(ActivityReport activityReport);
    }
}