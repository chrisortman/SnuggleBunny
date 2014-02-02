using System;
using System.Collections.Generic;

namespace SnuggleBunny.Activity
{
    public class ActivityReport
    {
        private readonly List<FinancialTransaction> _transactions = new List<FinancialTransaction>(); 
        public void Load(string transactionData)
        {
            
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