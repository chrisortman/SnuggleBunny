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

    /// <summary>
    ///     Set of helper methods for parsing data from CsvReaders.
    /// </summary>
    /// <remarks>
    ///     Implemented as extension methods so that any interface
    ///     implementations can get them for free
    /// </remarks>
    public static class CsvReaderHelpers
    {
        /// <summary>
        /// Gets the given field as a string value
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        public static string GetString(this ICsvReader reader, int fieldIndex)
        {
            return reader[fieldIndex];
        }

        /// <summary>
        /// Gets the given field as an Int value.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        /// <exception cref="CsvParseException">If the value cannot be parsed</exception>
        public static int GetInt(this ICsvReader reader, int fieldIndex)
        {
            int x;
            if (Int32.TryParse(reader[fieldIndex], out x))
            {
                return x;
            }
            throw FieldParseException(fieldIndex);
        }

        /// <summary>
        /// Gets the given field as a decimal
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        /// <exception cref="CsvParseException">If the value cannot be parsed</exception>
        public static decimal GetDecimal(this ICsvReader reader, int fieldIndex)
        {
            decimal x;
            if (Decimal.TryParse(reader[fieldIndex], out x))
            {
                return x;
            }
            throw FieldParseException(fieldIndex);
        }

        /// <summary>
        /// Getsd the given field as a DateTime
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        /// <exception cref="CsvParseException">If the value cannot be parsed</exception>
        public static DateTime GetDateTime(this ICsvReader reader, int fieldIndex)
        {
            DateTime x;
            if (DateTime.TryParse(reader[fieldIndex], out x))
            {
                return x;
            }
            throw FieldParseException(fieldIndex);
        }

        private static CsvParseException FieldParseException(int fieldIndex)
        {
            return new CsvParseException("Unable convert field " + fieldIndex + " to an integer", fieldIndex);
        }
    }
}