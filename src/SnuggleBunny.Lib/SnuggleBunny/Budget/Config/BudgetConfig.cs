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

namespace SnuggleBunny.Budget.Config
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds all the configurations and rules for your budget.
    /// </summary>
    public class BudgetConfig
    {
        private readonly Dictionary<string, SpendingCategory> _categories = new Dictionary<string, SpendingCategory>();

        /// <summary>
        /// Total amount of all montly income.
        /// </summary>
        public decimal MonthlyIncome { get; set; }

        /// <summary>
        /// Categories used to track spending.
        /// </summary>
        public Dictionary<string, SpendingCategory> Categories
        {
            get { return _categories; }
        }

        /// <summary>
        /// Creates a new category with the given limit
        /// </summary>
        /// <param name="categoryName">Name of the category</param>
        /// <param name="limit">Spending limit</param>
        public void DefineCategory(string categoryName, decimal limit)
        {
            _categories.Add(categoryName, new SpendingCategory
            {
                Name = categoryName,
                Limit = limit,
            });
        }
    }
}