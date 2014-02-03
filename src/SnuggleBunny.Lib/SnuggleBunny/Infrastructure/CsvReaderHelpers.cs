using System;

namespace SnuggleBunny.Infrastructure
{
    /// <summary>
    /// Set of helper methods for parsing data from CsvReaders.
    /// </summary>
    /// <remarks>
    /// Implemented as extension methods so that any interface
    /// implementations can get them for free
    /// </remarks>
    public static class CsvReaderHelpers
    {
        public static string GetString(this ICsvReader reader, int fieldIndex)
        {
            return reader[fieldIndex];
        }

        public static int GetInt(this ICsvReader reader,int fieldIndex)
        {
            int x;
            if (Int32.TryParse(reader[fieldIndex], out x))
            {
                return x;
            }
            else
            {
                throw FieldParseException(fieldIndex);
            }
        }

        public static decimal GetDecimal(this ICsvReader reader,int fieldIndex)
        {
            decimal x;
            if (Decimal.TryParse(reader[fieldIndex], out x))
            {
                return x;
            }
            else
            {
                throw FieldParseException(fieldIndex);
            }
        }

        public static DateTime GetDateTime(this ICsvReader reader, int fieldIndex)
        {
            DateTime x;
            if (DateTime.TryParse(reader[fieldIndex], out x))
            {
                return x;
            }
            else
            {
                throw FieldParseException(fieldIndex);
            }
        }
        private static CsvParseException FieldParseException(int fieldIndex)
        {
            return new CsvParseException("Unable convert field " + fieldIndex + " to an integer", fieldIndex);
        }
    }
}