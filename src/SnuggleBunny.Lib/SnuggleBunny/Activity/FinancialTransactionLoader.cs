using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace SnuggleBunny.Activity
{
    public class FinancialTransactionLoader
    {
        public IEnumerable<FinancialTransaction> LoadFile(string fileName)
        {
            var parser = new FinancialTransactionCsvParser();

            if (File.Exists(fileName))
            {
                using (var file = new StreamReader(fileName))
                {
                    string line = null;
                    while ((line = file.ReadLine()) != null)
                    {
                        FinancialTransaction parsedTransaction = null;
                        if (parser.TryParseLine(line, out parsedTransaction))
                        {
                            yield return parsedTransaction;
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
}