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

    /// <summary>
    /// Alert class used to report instances where spending in a category
    /// exceeded the alotted budget.
    /// </summary>
    /// <seealso cref="CategorySpendingLimitAnalyzer"/>
    public class CategorySpendingExceededAlert : IEquatable<CategorySpendingExceededAlert>, ISpendingAlert
    {
        public CategorySpendingExceededAlert(string category, int month, decimal spent, decimal allotted)
        {
            Guard.AgainstNull(category, "category");

            Category = category;
            Month = month;
            Spent = spent;
            Allotted = allotted;
        }

        /// <summary>
        /// The category where spending was exceeded
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// The month where the category spending was exceeded
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        /// The year where the category spending was exceeded
        /// </summary>
        public decimal Spent { get; private set; }

        /// <summary>
        /// The amount allotted to be spend in the category
        /// </summary>
        public decimal Allotted { get; private set; }

        // - Equals methods overridden to provide value semantics


        public bool Equals(CategorySpendingExceededAlert other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Category, other.Category) && Month == other.Month && Spent == other.Spent &&
                   Allotted == other.Allotted;
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
            return Equals((CategorySpendingExceededAlert) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Category.GetHashCode();
                hashCode = (hashCode*397) ^ Month;
                hashCode = (hashCode*397) ^ Spent.GetHashCode();
                hashCode = (hashCode*397) ^ Allotted.GetHashCode();
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
                Math.Abs(Allotted - Spent),
                Allotted,
                Spent);
        }
    }
}