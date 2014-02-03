using System;
using System.IO;
using System.Text;
using Shouldly;
using SnuggleBunny.Infrastructure;

namespace SnuggleBunny.Tests
{
    public static class TestHelpers
    {
        public static CsvReader CreateCsvReader(string[,] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    sb.Append(data[i, j]);
                    sb.Append(",");
                }

                sb.AppendLine();
            }

            var stringReader = new StringReader(sb.ToString());
            var csvReader = new CsvReader(stringReader);
            return csvReader;
        }
    }
}