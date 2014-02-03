using System;
using System.IO;
using Shouldly;
using SnuggleBunny.Infrastructure;
using Xunit;

namespace SnuggleBunny.Tests.Infrastructure
{
    public class CsvReaderTests
    {
        private string _data;
        private CsvReader _reader;

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
            string line;
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