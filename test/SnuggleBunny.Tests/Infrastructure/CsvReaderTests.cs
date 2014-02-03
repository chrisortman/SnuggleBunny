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

namespace SnuggleBunny.Tests.Infrastructure
{
    using System;
    using System.IO;
    using Shouldly;
    using SnuggleBunny.Infrastructure;
    using Xunit;

    public class CsvReaderTests
    {
        private readonly CsvReader _reader;
        private string _data;

        public CsvReaderTests()
        {
            _data = @"
a,2,2/4/2013,5.00,
a,2,2/4/2013,5.00,
";
            _reader = new CsvReader(new StringReader(_data));
        }

        [Fact]
        public void CanReadTheLines()
        {
            int lineCount = 0;
            while (_reader.Read())
            {
                lineCount++;
            }

            lineCount.ShouldBe(2);
        }

        [Fact]
        public void CanAccessFields()
        {
            _reader.Read();
            _reader[0].ShouldBe("a");
            _reader[1].ShouldBe("2");
            _reader[2].ShouldBe("2/4/2013");
            _reader[3].ShouldBe("5.00");
        }

        [Fact]
        public void AccessingFieldWithoutReadingWillThrow()
        {
            Should.Throw<InvalidOperationException>(() => _reader[0].ShouldBe("a"));
        }

        [Fact]
        public void CanReadAFieldAsInt()
        {
            _reader.Read();
            _reader.GetInt(1).ShouldBe(2);
        }

        [Fact]
        public void CanReadFieldAsDecimal()
        {
            _reader.Read();
            _reader.GetDecimal(3).ShouldBe(5M);
        }

        [Fact]
        public void CanReadDateTime()
        {
            _reader.Read();
            _reader.GetDateTime(2).ShouldBe(new DateTime(2013, 2, 4));
        }

        [Fact]
        public void ReadingInvalidDataAsIntWillThrow()
        {
            _reader.Read();
            Should.Throw<CsvParseException>(() => _reader.GetInt(0));
        }

        [Fact]
        public void EOFIsTrueWhenNoMoreData()
        {
            _reader.Read(); //first line
            _reader.EOF.ShouldBe(false);
            _reader.Read(); //second line
            _reader.EOF.ShouldBe(true);
        }
    }
}