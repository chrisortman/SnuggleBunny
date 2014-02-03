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

namespace SnuggleBunny.Budget.Config.Builders
{
    /// <summary>
    /// Used to provide a fluent inteface for configuring a budget.
    /// </summary>
    public class BudgetConfigBuilder
    {
        private readonly BudgetConfig _config;

        public BudgetConfigBuilder(BudgetConfig config)
        {
            Guard.AgainstNull(config,"config");

            _config = config;
        }

        /// <summary>
        /// Creates a new spending category.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [NotNull]
        public CategoryBuilder Category(string categoryName)
        {
            var builder = new CategoryBuilder(categoryName, _config);
            return builder;
        }

        /// <summary>
        /// Sets the available income per month.
        /// </summary>
        /// <param name="amount"></param>
        public void IncomePerMonth(decimal amount)
        {
            Guard.Requires(amount > 0,"Monthly income must be positive");
            _config.MonthlyIncome = amount;
        }
    }
}