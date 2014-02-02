using System;
using Shouldly;
using SnuggleBunny.Activity;
using Xunit;

namespace SnuggleBunny.Tests.Activity
{
    public class FinancialTransactionCsvParserTests
    {
        public class TheTryParseLineMethod
        {
            [Fact]
            public void ReturnsFalseWhenNotEnoughColumns()
            {
                var parser = new FinancialTransactionCsvParser();
                FinancialTransaction transaction;
                parser.TryParseLine("1/1/2014,Walmart,1",out transaction).ShouldBe(false);
            }

            [Fact]
            public void InitializesAllPropertiesOfTheFinancialTransaction()
            {
                var parser = new FinancialTransactionCsvParser();
                FinancialTransaction transaction;
                parser.TryParseLine("1/15/2014,Walmart,10.00,clothing", out transaction).ShouldBe(true);
                transaction.ShouldNotBe(null);
                transaction.OccurredOn.ShouldBe(new DateTime(2014,1,15));
                transaction.Description.ShouldBe("Walmart");
                transaction.Amount.ShouldBe(10M);
                transaction.Category.ShouldBe("clothing");
            }

            [Fact]
            public void ProvidesErrorMessageIfDateFormatIsInvalid()
            {
                var parser = new FinancialTransactionCsvParser();
                FinancialTransaction transaction;
                parser.TryParseLine("15/1/2014,Walmart,10.00,clothing",out transaction).ShouldBe(false);
                parser.ErrorMessage.ShouldBe("Invalid date format '15/1/2014'");
            }

            [Fact]
            public void ProvidesErrorMessageIfDecimalHasBogusCharacters()
            {
                var parser = new FinancialTransactionCsvParser();
                FinancialTransaction transaction;
                parser.TryParseLine("1/14/2014,Walmart,XX0.00,clothing",out transaction).ShouldBe(false);
                parser.ErrorMessage.ShouldBe("Invalid money format 'XX0.00'");
            }
        }
    }
}