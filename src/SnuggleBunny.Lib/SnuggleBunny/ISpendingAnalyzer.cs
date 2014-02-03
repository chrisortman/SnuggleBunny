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

namespace SnuggleBunny.Budget
{
    using System.Collections.Generic;
    using Activity;
    using Config;

    /// <summary>
    /// Contract for analyzers to be plugged in.
    /// </summary>
    public interface ISpendingAnalyzer
    {
        /// <summary>
        /// Initalize the analyzer with any settings provided in the budget config.
        /// </summary>
        /// <param name="config"></param>
        void Initialize(BudgetConfig config);

        /// <summary>
        /// Run analysis on the activity report.
        /// </summary>
        /// <param name="activityReport"></param>
        /// <returns></returns>
        IEnumerable<ISpendingAlert> Analyze(ActivityReport activityReport);
    }
}