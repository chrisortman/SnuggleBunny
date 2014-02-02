using System.Collections.Generic;

namespace SnuggleBunny.Budget.Config
{
    public class BudgetConfig
    {
        private readonly List<SpendingCategory> _categories = new List<SpendingCategory>();

        public void DefineCategory(string categoryName, decimal limit)
        {
            _categories.Add(new SpendingCategory()
            {
                Name = categoryName,
                Limit = limit,
            });
        }

        public decimal MonthlyIncome { get; set; }

        public List<SpendingCategory> Categories
        {
            get { return _categories; }
        }
    }
}