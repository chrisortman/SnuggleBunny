using System;
using System.Collections.Generic;
using System.Linq;

namespace SnuggleBunny.Activity
{

    public class ActivityReport
    {
        private List<FinancialTransaction> _transactions;

        public ActivityReport()
        {
            _transactions = new List<FinancialTransaction>();
        }

        public void Load(string transactionFile)
        {
            Guard.AgainstNull(transactionFile,"transactionFile");
            Guard.Against<ArgumentException>(transactionFile.IsBlank(),"transactionFile must not be blank");

            var loader = new FinancialTransactionLoader();
            _transactions = loader.LoadFile(transactionFile).ToList();
        }

        public void AddTransaction(DateTime dateTime, string description, decimal amount, string category)
        {
            Guard.AgainstNull(description,"description");
            Guard.AgainstNull(category, "category");

            _transactions.Add(new FinancialTransaction()
            {
                OccurredOn = dateTime,
                Description = description,
                Amount = amount,
                Category = category,
            });
        }

        public IEnumerable<FinancialTransaction> AllTransactions()
        {
            return _transactions;
        }

        public IEnumerable<IGrouping<MonthGroup, FinancialTransaction>> TransactionsByMonth()
        {
            return _transactions
                .OrderBy(x => x.OccurredOn.Year)
                .ThenBy(x => x.OccurredOn.Month)
                .GroupBy(MonthGroup.ForTransaction);
        }

        public IEnumerable<IGrouping<MonthCategoryGroup,FinancialTransaction>> TransactionsByMonthAndCategory()
        {
            return _transactions
                .OrderBy(x => x.OccurredOn.Year)
                .ThenBy(x => x.OccurredOn.Month)
                .ThenBy(x => x.Category)
                .GroupBy(MonthCategoryGroup.ForTransaction);
        }
    }

    public struct MonthCategoryGroup
    {
        public static MonthCategoryGroup ForTransaction(FinancialTransaction transaction)
        {
            return new MonthCategoryGroup(transaction.OccurredOn.Month,transaction.OccurredOn.Year,transaction.Category);
        }

        public MonthCategoryGroup(int month, int year, string category) : this()
        {
            Month = month;
            Year = year;
            Category = category;
        }

        public int Month { get; set; }
        public int Year { get; set; }
        public string Category { get; set; }
    }

    public struct MonthGroup : IEquatable<MonthGroup>
    {
        public static MonthGroup ForTransaction(FinancialTransaction transaction)
        {
            return new MonthGroup(transaction.OccurredOn.Year,transaction.OccurredOn.Month);
        }

        public MonthGroup(int year, int month) : this()
        {
            Year = year;
            Month = month;
        }        

        public int Year { get; set; }
        public int Month { get; set; }

        public bool Equals(MonthGroup other)
        {
            return Year == other.Year && Month == other.Month;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MonthGroup && Equals((MonthGroup) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Year*397) ^ Month;
            }
        }

        public static bool operator ==(MonthGroup left, MonthGroup right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MonthGroup left, MonthGroup right)
        {
            return !left.Equals(right);
        }
    }
}