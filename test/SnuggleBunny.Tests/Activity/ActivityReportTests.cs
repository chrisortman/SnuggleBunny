using System;
using System.Linq;
using Shouldly;
using SnuggleBunny.Activity;
using SnuggleBunny.Budget.Analyzers;
using Xunit;
using YamlDotNet.Core.Tokens;

namespace SnuggleBunny.Tests.Activity
{
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

        public class TheTransactionsByMonthMethod
        {
            [Fact]
            public void GroupsTransactionsByMonth()
            {
                var activityReport = new ActivityReport();
                activityReport.AddTransaction(new DateTime(2014,1,4),"Walmart",10M,"grocery" );
                activityReport.AddTransaction(new DateTime(2014,1,5),"Walmart",10M,"grocery" );
                activityReport.AddTransaction(new DateTime(2014,2,4),"Walmart",10M,"grocery" );
                activityReport.AddTransaction(new DateTime(2014,2,5),"Walmart",10M,"grocery" );

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

        public class TheTransactionsByMonthAndCategoryMethod
        {
            [Fact]
            public void GroupsTransactionsByMonthAndCategory()
            {
                
                var activityReport = new ActivityReport();
                activityReport.AddTransaction(new DateTime(2014,1,4),"Walmart",10M,"grocery" );
                activityReport.AddTransaction(new DateTime(2014,1,5),"Walmart",10M,"clothing" );
                activityReport.AddTransaction(new DateTime(2014,2,4),"Walmart",10M,"grocery" );
                activityReport.AddTransaction(new DateTime(2014,2,5),"Walmart",10M,"grocery" );

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
    }
}