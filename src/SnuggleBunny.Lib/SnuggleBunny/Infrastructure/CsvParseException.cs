using System;

namespace SnuggleBunny.Infrastructure
{
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