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

namespace SnuggleBunny.Activity
{
    using System;
    using Infrastructure;

    /// <summary>
    /// Responsible for parsing <see cref="FinancialTransaction"/> data out of
    /// a CSV file
    /// </summary>
    public sealed class FinancialTransactionCsvParser : IDisposable
    {
        private static readonly string[] _fieldNames = {"OccurredOn", "Description", "Amount", "Category"};
        private readonly ICsvReader _csvReader;

        public FinancialTransactionCsvParser(ICsvReader reader)
        {
            _csvReader = reader;
        }

        /// <summary>
        /// If an error occurrs parsing a value it will be available here
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// True if there are more rows to process, otherwise false.
        /// </summary>
        /// <returns></returns>
        public bool HasMoreRows()
        {
            return !_csvReader.EOF;
        }

        /// <summary>
        /// Attempts to read a transaction line.
        /// If it fails <see cref=""></typeparam>"/>
        /// </summary>
        /// <returns>The transaction if it parses successfully otherwise Nothing</returns>
        public Maybe<FinancialTransaction> MaybeReadTransaction()
        {
            if (_csvReader.Read())
            {
                try
                {
                    var transaaction = new FinancialTransaction
                    {
                        OccurredOn = _csvReader.GetDateTime(0),
                        Description = _csvReader.GetString(1),
                        Amount = _csvReader.GetDecimal(2),
                        Category = _csvReader.GetString(3),
                    };

                    return transaaction.ToMaybe();
                }
                catch (CsvParseException parseException)
                {
                    Guard.Requires(parseException.FieldIndex < _fieldNames.Length, "Invalid field index in error");

                    ErrorMessage = String.Format("Invalid format for {0}. Data was {1}",
                        _fieldNames[parseException.FieldIndex], _csvReader[parseException.FieldIndex]);
                }
            }

            return Maybe<FinancialTransaction>.Nothing;
        }

        /// <summary>
        /// Disposes of this instance and the underlying reader.
        /// </summary>
        public void Dispose()
        {
            if (_csvReader != null)
            {
                _csvReader.Dispose();
            }
        }
    }
}