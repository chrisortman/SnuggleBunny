using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace SnuggleBunny.Budget.Config
{
    public class BudgetConfig
    {
        private readonly Dictionary<string,SpendingCategory> _categories = new Dictionary<string, SpendingCategory>();

        public void DefineCategory(string categoryName, decimal limit)
        {
            _categories.Add(categoryName,new SpendingCategory()
            {
                Name = categoryName,
                Limit = limit,
            });
        }

        public decimal MonthlyIncome { get; set; }

        public Dictionary<string,SpendingCategory> Categories
        {
            get { return _categories; }
        }
    }
}