using System;
using System.Collections.Generic;
using System.Linq;
using SnuggleBunny.Budget.Analyzers;

namespace SnuggleBunny.Activity
{
    public static class By
    {
        public static MonthGroup Month(FinancialTransaction transaction)
        {
            return new MonthGroup(transaction.OccurredOn.Year, transaction.OccurredOn.Month);
        }

        public static MonthCategoryGroup MonthAndCategory(FinancialTransaction transaction)
        {
            return new MonthCategoryGroup(transaction.OccurredOn.Month, transaction.OccurredOn.Year,
                transaction.Category);
        }

        public struct MonthGroup : IEquatable<MonthGroup>
        {
            public static MonthGroup ForTransaction(FinancialTransaction transaction)
            {
                return new MonthGroup(transaction.OccurredOn.Year, transaction.OccurredOn.Month);
            }

            public MonthGroup(int year, int month)
                : this()
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
                return obj is MonthGroup && Equals((MonthGroup)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Year * 397) ^ Month;
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

        public struct MonthCategoryGroup : IEquatable<MonthCategoryGroup>
        {
            public MonthCategoryGroup(int month, int year, string category)
                : this()
            {
                Guard.AgainstNull(category,"category");

                Month = month;
                Year = year;
                Category = category;
            }

            public int Month { get; set; }
            public int Year { get; set; }
            public string Category { get; set; }

            public bool Equals(MonthCategoryGroup other)
            {
                return Month == other.Month && Year == other.Year && string.Equals(Category, other.Category);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is MonthCategoryGroup && Equals((MonthCategoryGroup) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = Month;
                    hashCode = (hashCode*397) ^ Year;
                    hashCode = (hashCode*397) ^ Category.GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(MonthCategoryGroup left, MonthCategoryGroup right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(MonthCategoryGroup left, MonthCategoryGroup right)
            {
                return !left.Equals(right);
            }
        }
    }

    public static class Transactions
    {
        public static TotalledTransactionGroup<TGroup> Totalled<TGroup>(IGrouping<TGroup, FinancialTransaction> transactionGroup)
        {
            return new TotalledTransactionGroup<TGroup>(transactionGroup.Key, transactionGroup);
        }

        public struct TotalledTransactionGroup<TGroup>
        {
            public TGroup Group { get; private set; }
            public decimal Total { get; private set; }

            public TotalledTransactionGroup(TGroup @group, IEnumerable<FinancialTransaction> transactions) :this()
            {
                Group = @group;
                Total = transactions.Sum(x => x.Amount);
            }
        }
    }
}