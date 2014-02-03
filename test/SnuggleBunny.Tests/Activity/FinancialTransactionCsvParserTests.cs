using System;
using Shouldly;
using SnuggleBunny.Activity;
using Xunit;

namespace SnuggleBunny.Tests.Activity
{
    public class FinancialTransactionCsvParserTests
    {

        public class TheHasMoreRowsMethod
        {

            [Fact]
            public void GoesFromTrueToFalseWhenEOFIsReached()
            {
                var csvReader = TestHelpers.CreateCsvReader(new[,]
                {
                    {"1/2/2014", "Walmart", "10.00", "grocery"},
                });

                var parser = new FinancialTransactionCsvParser(csvReader);
                parser.HasMoreRows().ShouldBe(true);
                csvReader.Read();
                parser.HasMoreRows().ShouldBe(false);

            }
        }

        public class TheTryReadTransactionMethod
        {
            [Fact]
            public void WillCreateTransactionWhenValidData()
            {
                var csvReader = TestHelpers.CreateCsvReader(new[,]
                {
                    {"1/2/2014", "Walmart", "10.00", "grocery"},
                });

                var parser = new FinancialTransactionCsvParser(csvReader);
                var transaction = parser.MaybeReadTransaction();
                transaction.IsSomething().ShouldBe(true);
                transaction.Value.OccurredOn.ShouldBe(new DateTime(2014,1,2));
                transaction.Value.Description.ShouldBe("Walmart");
                transaction.Value.Amount.ShouldBe(10M);
                transaction.Value.Category.ShouldBe("grocery");

            }
        }
    }
}