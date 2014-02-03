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

namespace SnuggleBunny.Infrastructure
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Error throw when we try to convert a CsvField to a given type but
    /// are unable to.
    /// </summary>
    [Serializable]
    public class CsvParseException : Exception
    {
        private readonly int _fieldIndex;

        public CsvParseException(string message, int fieldIndex) : base(message)
        {
            _fieldIndex = fieldIndex;
        }

        /// <summary>
        /// Index of the failed field. Use to report more helpful error
        /// messages.
        /// </summary>
        public int FieldIndex
        {
            get { return _fieldIndex; }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FieldIndex", FieldIndex);
            base.GetObjectData(info, context);
        }
    }
}