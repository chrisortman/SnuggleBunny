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

namespace SnuggleBunny
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Activity;
    using Budget.Config;
    using Budget.Config.Builders;

    /// <summary>
    /// BudgetTool
    /// Allows you to define rules for your budget to follow and import your financial transactions
    /// for analysis.
    /// </summary>
    public class BudgetTool
    {
        private readonly List<ISpendingAnalyzer> _analyzers;
        private readonly BudgetConfig _config;

        public BudgetTool(string configFile)
        {
            Guard.AgainstNull(configFile, "configFile");
            Guard.Against<ArgumentException>(configFile.IsBlank(), "configFile should not be blank");

            var loader = new BudgetConfigLoader();
            _config = loader.LoadFile(configFile);
            _analyzers = new List<ISpendingAnalyzer>();
        }

        public BudgetTool()
        {
            _config = new BudgetConfig();
            _analyzers = new List<ISpendingAnalyzer>();
        }

        /// <summary>
        /// Programmatically configure your budget 
        /// </summary>
        /// <param name="action"></param>
        public void Configure(Action<BudgetConfigBuilder> action)
        {
            Guard.AgainstNull(action, "action");

            var builder = new BudgetConfigBuilder(_config);
            action(builder);
        }

        /// <summary>
        /// Add the given analyzer
        /// </summary>
        /// <param name="analyzer"></param>
        public void AddAnalyzer(ISpendingAnalyzer analyzer)
        {
            Guard.AgainstNull(analyzer, "analyzer");
            _analyzers.Add(analyzer);
        }

        /// <summary>
        /// Analyze your financial activity.
        /// </summary>
        /// <param name="activityReport"></param>
        /// <returns></returns>
        public IReadOnlyCollection<ISpendingAlert> Analyze(ActivityReport activityReport)
        {
            Guard.AgainstNull(activityReport, "activityReport");

            _analyzers.ForEach(x => x.Initialize(_config));
            return new ReadOnlyCollection<ISpendingAlert>(
                _analyzers.SelectMany(x => x.Analyze(activityReport)).ToList());
        }
    }
}