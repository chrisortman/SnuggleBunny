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

            if (File.Exists(fileName))
            {
                var parser = new FinancialTransactionCsvParser(fileName);
                while (parser.HasMoreRows())
                {
                    var transaction = parser.TryReadTransaction();
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