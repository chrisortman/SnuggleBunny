using System;
using System.Runtime.Serialization;

namespace SnuggleBunny.Infrastructure
{
    [Serializable]
    public class CsvParseException : Exception
    {
        private readonly int _fieldIndex;

        public CsvParseException(string message, int fieldIndex) : base(message)
        {
            _fieldIndex = fieldIndex;
        }

        public int FieldIndex
        {
            get { return _fieldIndex; }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FieldIndex",FieldIndex);
            base.GetObjectData(info, context);
        }
    }
}