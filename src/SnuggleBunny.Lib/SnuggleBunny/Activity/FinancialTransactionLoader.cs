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
            if (File.Exists(fileName))
            {
                using (var file = new StreamReader(fileName))
                {
                    string line = null;
                    while ((line = file.ReadLine()) != null)
                    {
                        Exception parsingError = null;
                        try
                        {
                            FinancialTransaction parsedTransaction = null;
                            try
                            {
                                var parts = line.Split(',');
                                var occurredOn = DateTime.Parse(parts[0]);
                                var amount = Decimal.Parse(parts[2]);

                                parsedTransaction = new FinancialTransaction()
                                {
                                    OccurredOn = occurredOn,
                                    Description = parts[0],
                                    Amount = amount,
                                    Category = parts[3]
                                };
                            }
                            catch (Exception ex)
                            {
                                parsingError = ex;
                                throw;
                            }

                            yield return parsedTransaction;
                        }
                        finally
                        {
                            if (parsingError != null)
                            {
                                throw new ConfigurationErrorsException("Invalid transaction file", parsingError);
                            }
                        }
                    }
                }
            }
        }
    }
}