﻿#region LICENSE

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
    using Shouldly;
    using SnuggleBunny.Activity;
    using Xunit;

    public class FinancialTransactionCsvParserTests
    {
        public class TheHasMoreRowsMethod
        {
            [Fact]
            public void GoesFromTrueToFalseWhenEOFIsReached()
            {
                var csvReader = TestHelpers.CreateCsvReader(new[,]
                {
                    {"1/2/2014", "Walmart", "10.00", "grocery"}
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
                    {"1/2/2014", "Walmart", "10.00", "grocery"}
                });

                var parser = new FinancialTransactionCsvParser(csvReader);
                var transaction = parser.MaybeReadTransaction();
                transaction.IsSomething().ShouldBe(true);
                transaction.Value.OccurredOn.ShouldBe(new DateTime(2014, 1, 2));
                transaction.Value.Description.ShouldBe("Walmart");
                transaction.Value.Amount.ShouldBe(10M);
                transaction.Value.Category.ShouldBe("grocery");
            }
        }
    }
}