namespace SnuggleBunny.Budget.Config.Builders
{
    public class BudgetConfigBuilder
    {
        private readonly BudgetConfig _config;

        public BudgetConfigBuilder(BudgetConfig config)
        {
            _config = config;
        }

        public CategoryBuilder Category(string categoryName)
        {
            var builder = new CategoryBuilder(categoryName,_config);
            return builder;
        }

        public void IncomePerMonth(decimal amount)
        {
            _config.MonthlyIncome = amount;
        }
    }
}