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

namespace SnuggleBunny.Tests.Activity
{
    using System;
    using System.Linq;
    using Shouldly;
    using SnuggleBunny.Activity;
    using Xunit;

    public class ActivityReportTests
    {
        public class TheLoadMethod
        {
            [Fact]
            public void EnsuresTransactionFileNotNull()
            {
                var activityReport = new ActivityReport();
                Should.Throw<ArgumentNullException>(() => activityReport.Load(null));
            }

            [Fact]
            public void EnsuresTransactionFileNotBlank()
            {
                var activityReport = new ActivityReport();
                Should.Throw<ArgumentException>(() => activityReport.Load(""));
            }
        }

        public class TheTransactionsByMonthAndCategoryMethod
        {
            [Fact]
            public void GroupsTransactionsByMonthAndCategory()
            {
                var activityReport = new ActivityReport();
                activityReport.AddTransaction(new DateTime(2014, 1, 4), "Walmart", 10M, "grocery");
                activityReport.AddTransaction(new DateTime(2014, 1, 5), "Walmart", 10M, "clothing");
                activityReport.AddTransaction(new DateTime(2014, 2, 4), "Walmart", 10M, "grocery");
                activityReport.AddTransaction(new DateTime(2014, 2, 5), "Walmart", 10M, "grocery");

                var groups = activityReport.GroupTransactions(By.MonthAndCategory);
                var keys = groups.Select(x => x.Key)
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .ThenBy(x => x.Category)
                    .ToList();

                keys[0].Month.ShouldBe(1);
                keys[0].Year.ShouldBe(2014);
                keys[0].Category.ShouldBe("clothing");

                keys[1].Month.ShouldBe(1);
                keys[1].Year.ShouldBe(2014);
                keys[1].Category.ShouldBe("grocery");

                keys[2].Month.ShouldBe(2);
                keys[2].Year.ShouldBe(2014);
                keys[2].Category.ShouldBe("grocery");
            }
        }

        public class TheTransactionsByMonthMethod
        {
            [Fact]
            public void GroupsTransactionsByMonth()
            {
                var activityReport = new ActivityReport();
                activityReport.AddTransaction(new DateTime(2014, 1, 4), "Walmart", 10M, "grocery");
                activityReport.AddTransaction(new DateTime(2014, 1, 5), "Walmart", 10M, "grocery");
                activityReport.AddTransaction(new DateTime(2014, 2, 4), "Walmart", 10M, "grocery");
                activityReport.AddTransaction(new DateTime(2014, 2, 5), "Walmart", 10M, "grocery");

                var groups = activityReport.GroupTransactions(By.Month);
                var keys = groups.Select(x => x.Key)
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .ToList();

                keys[0].Month.ShouldBe(1);
                keys[0].Year.ShouldBe(2014);

                keys[1].Month.ShouldBe(2);
                keys[1].Year.ShouldBe(2014);
            }
        }
    }
}