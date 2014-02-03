using System;
using SnuggleBunny.Infrastructure;

namespace SnuggleBunny.Activity
{
    public sealed class FinancialTransactionCsvParser : IDisposable
    {
        private readonly ICsvReader _csvReader;
        private static readonly string[] _fieldNames = new[] {"OccurredOn", "Description", "Amount", "Category"};

        public FinancialTransactionCsvParser(ICsvReader reader)
        {
            _csvReader = reader;
        }

        public string ErrorMessage { get; private set; }

        public bool HasMoreRows()
        {
            return !_csvReader.EOF;
        }

        public Maybe<FinancialTransaction> MaybeReadTransaction()
        {
            if (_csvReader.Read())
            {
                try
                {
                    var transaaction = new FinancialTransaction()
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

        public void Dispose()
        {
            if (_csvReader != null)
            {
                _csvReader.Dispose();
            }
        }
    }
}