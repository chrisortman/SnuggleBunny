using System;
using System.IO;

namespace SnuggleBunny.Infrastructure
{
    public class CsvReader : ICsvReader
    {
        private readonly TextReader _innerReader;
        private string _currentLine;
        private string[] _currentLineParts;

        public CsvReader(TextReader stringReader)
        {
            _innerReader = stringReader;
        }

        public string this[int fieldIndex]
        {
            get
            {
                EnsureReadData();
                return _currentLineParts[fieldIndex];
            }
        }

        public bool EOF
        {
            get
            {
                return _innerReader.Peek() == -1;
            }
        }

        public bool Read()
        {
            string line = null;
            while (line.IsBlank() && !EOF)
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

      

        private void EnsureReadData()
        {
            if (_currentLine == null)
            {
                throw new InvalidOperationException("You must read before accessing any fields");
            }
        }

    }
}