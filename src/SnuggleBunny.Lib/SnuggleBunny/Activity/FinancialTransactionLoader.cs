using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using SnuggleBunny.Infrastructure;

namespace SnuggleBunny.Activity
{
    public class FinancialTransactionLoader
    {
        public IEnumerable<FinancialTransaction> LoadFile(string fileName)
        {
            return Load(new FileDataSource(fileName));
        }

        public IEnumerable<FinancialTransaction> Load(IFileDataSource dataSource)
        {
            if (dataSource.Exists())
            {
                var parser = new FinancialTransactionCsvParser(dataSource.ReadCsv());
                while (parser.HasMoreRows())
                {
                    var transaction = parser.MaybeReadTransaction();
                    if (transaction.IsSomething())
                    {
                        yield return transaction.Value;
                    }
                    else
                    {
                        break;
                    }
                }

                if (!parser.ErrorMessage.IsBlank())
                {
                    throw new ConfigurationErrorsException("Invalid transaction file");
                }
            }
        }
    }
}