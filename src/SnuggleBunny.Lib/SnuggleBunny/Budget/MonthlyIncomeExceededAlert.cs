using System;
using System.Globalization;

namespace SnuggleBunny.Budget
{
    public class MonthlyIncomeExceededAlert : IEquatable<MonthlyIncomeExceededAlert>, ISpendingAlert
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
}