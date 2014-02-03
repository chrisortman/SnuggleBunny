using System;

namespace SnuggleBunny.Activity
{
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

        public int Month { get; private set; }
        public int Year { get; private set; }
        public string Category { get; private set; }

        public bool Equals(MonthCategoryGroup other)
        {
            return Month == other.Month && Year == other.Year && String.Equals(Category, other.Category);
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