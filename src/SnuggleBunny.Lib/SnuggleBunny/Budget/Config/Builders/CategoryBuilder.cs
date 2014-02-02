namespace SnuggleBunny.Budget.Config.Builders
{
    public class CategoryBuilder
    {
        private readonly string _categoryName;
        private readonly BudgetConfig _config;

        public CategoryBuilder(string categoryName, BudgetConfig config)
        {
            _categoryName = categoryName;
            _config = config;
        }

        public void LimitTo(decimal amount)
        {
            _config.DefineCategory(_categoryName,amount);
        }
    }
}