using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using Shouldly;
using Xunit;

namespace SnuggleBunny.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void DetectsOverespendingInACategory()
        {
            var tool = new BudgetTool("budget_config.yml");
            var spendingReport = new SpendingReport();
            spendingReport.Load("transactions.csv");

            var alerts = tool.Analyze(spendingReport);

            alerts.ShouldContain(
                x => x.ToString() == "[OVERAGE] - Spending for clothing in Feb exceeded budget by $150.00 ($200.00/$350.00)");

        }
    }

    public class BudgetToolTests
    {
        public class TheAnalyzeMethod
        {
            [Fact]
            public void DetectsOverspendingInACategory()
            {
                var tool = new BudgetTool();
                tool.DefineCategory("clothing", limit: 200M);

                var spendingReport = new SpendingReport();
                spendingReport.AddTransaction(new DateTime(2014, 2, 2), "Gap", 201M, "clothing");

                var alerts = tool.Analyze(spendingReport);
                alerts.ShouldContain(new CategorySpendingExceededAlert("clothing",2,201M,200M));

            }

            [Fact]
            public void DetectsMonthsWhereSpendingExceedsMonthlyIncome()
            {
                var tool = new BudgetTool();
                tool.MonthlyIncome(500M);

                var spendingReport = new SpendingReport();
                spendingReport.AddTransaction(new DateTime(2014,1,2),"Walmart",499M,"Grocery");
                spendingReport.AddTransaction(new DateTime(2014,2,2),"Walmart",501M,"Grocery");

                var alerts = tool.Analyze(spendingReport);

                alerts.ShouldContain(new MonthlyIncomeExceededAlert(2,501M,500M));
            }
        }
    }

    public interface IBudgetAlert 
    {
        string Describe();
    }
    public class MonthlyIncomeExceededAlert : IEquatable<MonthlyIncomeExceededAlert>, IBudgetAlert
    {
        private readonly int _month;
        private readonly decimal _spent;
        private readonly decimal _available;

        public MonthlyIncomeExceededAlert(int month, decimal spent, decimal available)
        {
            _month = month;
            _spent = spent;
            _available = available;
        }

        public int Month
        {
            get { return _month; }
        }

        public decimal Spent
        {
            get { return _spent; }
        }

        public decimal Available
        {
            get { return _available; }
        }

        public bool Equals(MonthlyIncomeExceededAlert other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _month == other._month && _spent == other._spent && _available == other._available;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MonthlyIncomeExceededAlert) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _month;
                hashCode = (hashCode*397) ^ _spent.GetHashCode();
                hashCode = (hashCode*397) ^ _available.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(MonthlyIncomeExceededAlert left, MonthlyIncomeExceededAlert right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MonthlyIncomeExceededAlert left, MonthlyIncomeExceededAlert right)
        {
            return !Equals(left, right);
        }

        public string Describe()
        {
            return ToString();
        }

        public override string ToString()
        {
            return String.Format("[SPENDING] - Spending for {0} exceeded monthly income by {1:C} ({2:C}/{3:C})",
               CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Month),
               Spent - Available,
               Available,
               Spent);
        }
    }


    public struct CategorySpendingExceededAlert : IEquatable<CategorySpendingExceededAlert>, IBudgetAlert
    {

        public string Category { get; private set; }
        public int Month { get; private set; }
        public decimal Spent { get; private set;}
        public decimal Alloted { get; private set; }

        public CategorySpendingExceededAlert(string category, int month, decimal spent, decimal alloted) : this()
        {
            Guard.AgainstNull(category,"category");

            Category = category;
            Month = month;
            Spent = spent;
            Alloted = alloted;
        }

        public bool Equals(CategorySpendingExceededAlert other)
        {
            return string.Equals(Category, other.Category) && Month == other.Month && Spent == other.Spent && Alloted == other.Alloted;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CategorySpendingExceededAlert && Equals((CategorySpendingExceededAlert) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Category.GetHashCode();
                hashCode = (hashCode*397) ^ Month;
                hashCode = (hashCode*397) ^ Spent.GetHashCode();
                hashCode = (hashCode*397) ^ Alloted.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(CategorySpendingExceededAlert left, CategorySpendingExceededAlert right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CategorySpendingExceededAlert left, CategorySpendingExceededAlert right)
        {
            return !left.Equals(right);
        }


        public override string ToString()
        {
            return String.Format("[OVERAGE] - Spending for {0} in {1} exceeded budget by {2:C} ({3:C}/{4:C})",
                                  Category,
                                  CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Month),
                                  Math.Abs(Alloted-Spent),
                                  Alloted,
                                  Spent);
        }

        public string Describe()
        {
            return ToString();
        }
    }

    public class SpendingReport
    {
        private readonly List<SpendingTransaction> _transactions = new List<SpendingTransaction>(); 
        public void Load(string transactionData)
        {
            
        }

        public void AddTransaction(DateTime dateTime, string description, decimal amount, string category)
        {
            _transactions.Add(new SpendingTransaction()
            {
                OccurredOn = dateTime,
                Description = description,
                Amount = amount,
                Category = category,
            });
        }

        public IEnumerable<SpendingTransaction> Transactions()
        {
            return _transactions;
        }
    }

    public class SpendingTransaction
    {
        public DateTime OccurredOn { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public string Category { get; set; }

    }

    public interface ISpendingAnalyzer
    {
        void Initialize(BudgetConfig config);
        IEnumerable<IBudgetAlert> Analyze(SpendingReport spendingReport);
    }

    public class OverspendInCategoryForMonthAnalyzer : ISpendingAnalyzer
    {
        private List<SpendingCategory> _categories;

        public void Initialize(BudgetConfig config)
        {
            _categories = config.Categories;
        }

        public IEnumerable<IBudgetAlert> Analyze(SpendingReport spendingReport)
        {
            var spendingByCategoryMonth =
               from transaction in spendingReport.Transactions()
               group transaction by
                   new
                   {
                       Year = transaction.OccurredOn.Year,
                       Month = transaction.OccurredOn.Month,
                       Category = transaction.Category
                   }
                   into byMonth
                   select new
                   {
                       Group = byMonth.Key,
                       Total = byMonth.Sum(x => x.Amount)
                   };



            var overages = from spent in spendingByCategoryMonth
                           join cat in _categories on spent.Group.Category equals cat.Name
                           where spent.Total > cat.Limit
                           select new CategorySpendingExceededAlert(cat.Name, spent.Group.Month, spent.Total, cat.Limit);

            return overages.Cast<IBudgetAlert>();
        }
    }

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

    public class MonthlySpendingVersusIncomeAnalyzer : ISpendingAnalyzer
    {
        private decimal _monthlyIncome;

        public void Initialize(BudgetConfig config)
        {
            _monthlyIncome = config.MonthlyIncome;
        }

        public IEnumerable<IBudgetAlert> Analyze(SpendingReport spendingReport)
        {
            var spendingByMonth =
             from transaction in spendingReport.Transactions()
             group transaction by
                 new
                 {
                     Year = transaction.OccurredOn.Year,
                     Month = transaction.OccurredOn.Month,
                 }
                 into byMonth
                 select new
                 {
                     Group = byMonth.Key,
                     Total = byMonth.Sum(x => x.Amount)
                 };

            var exceeded =
                from spent in spendingByMonth
                where spent.Total > _monthlyIncome
                select new MonthlyIncomeExceededAlert(spent.Group.Month, spent.Total, _monthlyIncome);

            return exceeded;
        }
    }
    public class BudgetTool
    {
        private BudgetConfig _config = new BudgetConfig();
       

        public BudgetTool(string configFile)
        {
            
        }

        public BudgetTool()
        {
            
        }

        public IEnumerable<IBudgetAlert> Analyze(SpendingReport spendingReport)
        {
            var analyzers = new List<ISpendingAnalyzer>
            {
                new OverspendInCategoryForMonthAnalyzer(),
                new MonthlySpendingVersusIncomeAnalyzer(),
            };

            analyzers.ForEach(x => x.Initialize(_config));

            return analyzers.SelectMany(x => x.Analyze(spendingReport));

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

    public class SpendingCategory
    { 
        public string Name { get; set; }
        public decimal Limit { get; set; }
    }
}