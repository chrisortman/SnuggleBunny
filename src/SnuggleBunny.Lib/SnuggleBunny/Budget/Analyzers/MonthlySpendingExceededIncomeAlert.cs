#region LICENSE

// The MIT License (MIT)
// 
// Copyright (c) <year> <copyright holders>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

namespace SnuggleBunny.Budget.Analyzers
{
    using System;
    using System.Globalization;

    public class MonthlySpendingExceededIncomeAlert : IEquatable<MonthlySpendingExceededIncomeAlert>, ISpendingAlert
    {
        private readonly decimal _available;
        private readonly int _month;
        private readonly decimal _spent;

        public MonthlySpendingExceededIncomeAlert(int month, decimal spent, decimal available)
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

        public bool Equals(MonthlySpendingExceededIncomeAlert other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _month == other._month && _spent == other._spent && _available == other._available;
        }

        public string Describe()
        {
            return ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MonthlySpendingExceededIncomeAlert) obj);
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

        public static bool operator ==(MonthlySpendingExceededIncomeAlert left, MonthlySpendingExceededIncomeAlert right
            )
        {
            return Equals(left, right);
        }

        public static bool operator !=(MonthlySpendingExceededIncomeAlert left, MonthlySpendingExceededIncomeAlert right
            )
        {
            return !Equals(left, right);
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