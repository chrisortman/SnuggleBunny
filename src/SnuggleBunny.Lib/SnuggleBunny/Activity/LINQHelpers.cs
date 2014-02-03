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
    using System.Linq;

    /// <summary>
    /// Helpers used to group transactions in LINQ queries
    /// </summary>
    public static class By
    {

        /// <summary>
        /// Group transactions by month
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static MonthGroup Month(FinancialTransaction transaction)
        {
            return new MonthGroup(transaction.OccurredOn.Year, transaction.OccurredOn.Month);
        }

        /// <summary>
        /// Group transactions by month and category
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static MonthCategoryGroup MonthAndCategory(FinancialTransaction transaction)
        {
            return new MonthCategoryGroup(transaction.OccurredOn.Month, transaction.OccurredOn.Year,
                transaction.Category);
        }
    }

    /// <summary>
    /// Helpers used to select & summarize transactions
    /// </summary>
    public static class Transactions
    {
        /// <summary>
        /// Summarizes a group of transactions by totalling their amounts
        /// </summary>
        /// <typeparam name="TGroup"></typeparam>
        /// <param name="transactionGroup"></param>
        /// <returns></returns>
        public static TotalledTransactionGroup<TGroup> Totalled<TGroup>(
            IGrouping<TGroup, FinancialTransaction> transactionGroup)
        {
            return new TotalledTransactionGroup<TGroup>(transactionGroup.Key, transactionGroup);
        }
    }
}