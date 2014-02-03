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

    public class ActivityReport
    {
        private List<FinancialTransaction> _transactions;

        public ActivityReport()
        {
            _transactions = new List<FinancialTransaction>();
        }

        public void Load(string transactionFile)
        {
            Guard.AgainstNull(transactionFile, "transactionFile");
            Guard.Against<ArgumentException>(transactionFile.IsBlank(), "transactionFile must not be blank");

            var loader = new FinancialTransactionLoader();
            _transactions = loader.LoadFile(transactionFile).ToList();
        }

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

        public IEnumerable<FinancialTransaction> AllTransactions()
        {
            return _transactions;
        }

        public IEnumerable<IGrouping<TGroup, FinancialTransaction>> GroupTransactions<TGroup>(
            Func<FinancialTransaction, TGroup> grouper)
        {
            return _transactions.GroupBy(grouper);
        }
    }
}