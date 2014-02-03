using System.Configuration;
using System.Linq;
using NSubstitute;
using Shouldly;
using SnuggleBunny.Activity;
using SnuggleBunny.Infrastructure;
using Xunit;

namespace SnuggleBunny.Tests.Activity
{
    public class FinancialTransactionLoaderTests
    {
        public class TheLoadFileMethod
        {
            [Fact]
            public void WillLoadReportFromCsvFile()
            {
                var loader = new FinancialTransactionLoader();
                var transactions = loader.LoadFile(@"TestData\transactions.csv");

                transactions.Count().ShouldBe(4);
            }

            [Fact]
            public void WontCrashIfFileDoesNotExist()
            {
                var loader = new FinancialTransactionLoader();
                Should.NotThrow(() => loader.LoadFile("does_not_exist.csv").ToList());
            }

            [Fact]
            public void ThrowsConfigurationExceptionIfFileIsMalformed()
            {
                var loader = new FinancialTransactionLoader();
                Should.Throw<ConfigurationErrorsException>(() => loader.LoadFile(@"TestData\invalid_transactions.csv").ToList());
            }
        }

        public class TheLoadMethod
        {
            [Fact]
            public void WillNotAttemptToReadIfSourceDoesNotExist()
            {
                var loader = new FinancialTransactionLoader();
                var dataSource = Substitute.For<IFileDataSource>();
                dataSource.Exists().Returns(false);

                loader.Load(dataSource);

                dataSource.DidNotReceive().ReadCsv();
            }
        }
    }
}