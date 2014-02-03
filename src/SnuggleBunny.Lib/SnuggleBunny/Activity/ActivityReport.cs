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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Encapsulates financial activity for a user.
    /// </summary>
    public class ActivityReport
    {
        private List<FinancialTransaction> _transactions;

        public ActivityReport()
        {
            _transactions = new List<FinancialTransaction>();
        }

        /// <summary>
        /// Reads the transactions out of the given file.
        /// </summary>
        /// <param name="transactionFile"></param>
        public void Load(string transactionFile)
        {
            Guard.AgainstNull(transactionFile, "transactionFile");
            Guard.Against<ArgumentException>(transactionFile.IsBlank(), "transactionFile must not be blank");

            var loader = new FinancialTransactionLoader();
            _transactions = loader.LoadFile(transactionFile).ToList();
        }

        /// <summary>
        /// Add a transaction to this report
        /// </summary>
        /// <param name="dateTime">When the transaction ocurred</param>
        /// <param name="description">A description such as where the purchase was made</param>
        /// <param name="amount">The amount of the transaction</param>
        /// <param name="category">A category to assign the transaction to</param>
        public void AddTransaction(DateTime dateTime, string description, decimal amount, string category)
        {
            Guard.AgainstNull(description, "description");
            Guard.AgainstNull(category, "category");

            _transactions.Add(new FinancialTransaction
            {
                OccurredOn = dateTime,
                Description = description,
                Amount = amount,
                Category = category,
            });
        }

        /// <summary>
        /// Gets all transactions in the report
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FinancialTransaction> AllTransactions()
        {
            return _transactions;
        }

        /// <summary>
        /// Groups the transactions using the specified grouping function.
        /// <seealso cref="By"/>
        /// </summary>
        /// <typeparam name="TGroup">Type of the group key</typeparam>
        /// <param name="grouper">Function used to group transactions</param>
        /// <returns>Grouped transactions</returns>
        public IEnumerable<IGrouping<TGroup, FinancialTransaction>> GroupTransactions<TGroup>(
            Func<FinancialTransaction, TGroup> grouper)
        {
            return _transactions.GroupBy(grouper);
        }
    }
}