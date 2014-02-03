using System;
using System.Collections.Generic;
using System.Linq;

namespace SnuggleBunny.Activity
{

    public class ActivityReport
    {
        private List<FinancialTransaction> _transactions;

        public ActivityReport()
        {
            _transactions = new List<FinancialTransaction>();
        }

        public void Load(string transactionFile)
        {
            Guard.AgainstNull(transactionFile,"transactionFile");
            Guard.Against<ArgumentException>(transactionFile.IsBlank(),"transactionFile must not be blank");

            var loader = new FinancialTransactionLoader();
            _transactions = loader.LoadFile(transactionFile).ToList();
        }

        public void AddTransaction(DateTime dateTime, string description, decimal amount, string category)
        {
            Guard.AgainstNull(description,"description");
            Guard.AgainstNull(category, "category");

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