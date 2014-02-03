using System;
using Shouldly;
using SnuggleBunny.Activity;
using Xunit;

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
    }
}