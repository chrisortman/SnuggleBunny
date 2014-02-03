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

namespace SnuggleBunny.Activity
{
    using System;

    /// <summary>
    /// Group of transactions by month,year and category
    /// </summary>
    public struct MonthCategoryGroup : IEquatable<MonthCategoryGroup>
    {
        public MonthCategoryGroup(int month, int year, string category)
            : this()
        {
            Guard.AgainstNull(category, "category");

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