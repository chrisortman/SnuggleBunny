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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Group of transactions that have been totalled.
    /// </summary>
    /// <typeparam name="TGroup"></typeparam>
    public struct TotalledTransactionGroup<TGroup>
    {
        public TotalledTransactionGroup(TGroup @group, IEnumerable<FinancialTransaction> transactions) : this()
        {
            Group = @group;
            Total = transactions.Sum(x => x.Amount);
        }

        /// <summary>
        /// The key used to group these transactions
        /// </summary>
        public TGroup Group { get; private set; }

        /// <summary>
        /// The total amount of the grouped transactions.
        /// </summary>
        public decimal Total { get; private set; }
    }
}