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
    using System.Configuration;
    using System.Linq;
    using NSubstitute;
    using Shouldly;
    using SnuggleBunny.Activity;
    using SnuggleBunny.Infrastructure;
    using Xunit;

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
                Should.Throw<ConfigurationErrorsException>(
                    () => loader.LoadFile(@"TestData\invalid_transactions.csv").ToList());
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