using System;
using System.Globalization;

namespace SnuggleBunny.Budget.Analyzers
{
    public class CategorySpendingExceededAlert : IEquatable<CategorySpendingExceededAlert>, ISpendingAlert
    {

        public string Category { get; private set; }
        public int Month { get; private set; }
        public decimal Spent { get; private set;}
        public decimal Alloted { get; private set; }

        public CategorySpendingExceededAlert(string category, int month, decimal spent, decimal alloted) 
        {
            Guard.AgainstNull(category,"category");

            Category = category;
            Month = month;
            Spent = spent;
            Alloted = alloted;
        }

        public string Describe()
        {
            return ToString();
        }

        public bool Equals(CategorySpendingExceededAlert other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Category, other.Category) && Month == other.Month && Spent == other.Spent && Alloted == other.Alloted;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CategorySpendingExceededAlert) obj);
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
            return Equals(left, right);
        }

        public static bool operator !=(CategorySpendingExceededAlert left, CategorySpendingExceededAlert right)
        {
            return !Equals(left, right);
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
    }
}