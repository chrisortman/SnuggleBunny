using System;
using System.IO;
using Shouldly;
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
a,2,2/4/2013,5.00,
a,2,2/4/2013,5.00,
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

            lineCount.ShouldBe(6);
            
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
        public void CanReadAFieldAsInt()
        {
            _reader.Read();
            _reader.GetInt(1).ShouldBe(2);
        }
    }

    public class CsvReader
    {
        private StringReader _innerReader;
        private string _currentLine;
        private string[] _currentLineParts;

        public CsvReader(StringReader stringReader)
        {
            _innerReader = stringReader;
        }

        public string this[int fieldIndex]
        {
            get { return _currentLineParts[fieldIndex]; }
        }
        public bool Read()
        {
            string line = null;
            while (line.IsBlank() && _innerReader.Peek() != -1)
            {
                line = _innerReader.ReadLine();
            }
            _currentLine = line;

            if(!_currentLine.IsBlank())
            {
                _currentLineParts = _currentLine.Split(',');
            }
            return _currentLine != null;
        }

        public int GetInt(int fieldIndex)
        {
            int x;
            if (Int32.TryParse(this[fieldIndex], out x))
            {
                return x;
            }
            else
            {
                throw new CsvParseException("Unable convert field " + fieldIndex + " to an integer", fieldIndex);
            }
        }
    }

    public class CsvParseException : Exception
    {
        private int _fieldIndex;

        public CsvParseException(string message, int fieldIndex) : base(message)
        {
            _fieldIndex = fieldIndex;
        }

        public int FieldIndex
        {
            get { return _fieldIndex; }
        }
    }
}