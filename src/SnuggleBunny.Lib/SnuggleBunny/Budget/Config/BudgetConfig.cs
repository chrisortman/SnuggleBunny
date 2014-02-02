using System.Collections.Generic;

namespace SnuggleBunny.Budget.Config
{
    public class BudgetConfig
    {
        private readonly List<SpendingCategory> _categories = new List<SpendingCategory>();
        private decimal _monthlyIncome;

        public void DefineCategory(string clothing, decimal limit)
        {
            _categories.Add(new SpendingCategory()
            {
                Name = clothing,
                Limit = limit,
            });
        }

        public void IncomePerMonth(decimal amount)
        {
            _monthlyIncome = amount;
        }

        public decimal MonthlyIncome
        {
            get { return _monthlyIncome; }
        }

        public List<SpendingCategory> Categories
        {
            get { return _categories; }
        }
    }
}