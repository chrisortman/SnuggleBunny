using System;
using System.Collections.Generic;
using System.Linq;

namespace SnuggleBunny.Activity
{
    public class ActivityReport
    {
        private List<FinancialTransaction> _transactions = new List<FinancialTransaction>(); 
        public void Load(string transactionFile)
        {
            var loader = new FinancialTransactionLoader();
            _transactions = loader.LoadFile(transactionFile).ToList();
        }

        public void AddTransaction(DateTime dateTime, string description, decimal amount, string category)
        {
            _transactions.Add(new FinancialTransaction()
            {
                OccurredOn = dateTime,
                Description = description,
                Amount = amount,
                Category = category,
            });
        }

        public IEnumerable<FinancialTransaction> Transactions()
        {
            return _transactions;
        }
    }
}