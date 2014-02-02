using System;

namespace SnuggleBunny.Activity
{
    public class FinancialTransaction
    {
        public DateTime OccurredOn { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public string Category { get; set; }

    }
}