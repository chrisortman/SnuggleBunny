using System.Collections.Generic;
using System.Linq;

namespace SnuggleBunny.Activity
{
    public struct TotalledTransactionGroup<TGroup>
    {
        public TGroup Group { get; private set; }
        public decimal Total { get; private set; }

        public TotalledTransactionGroup(TGroup @group, IEnumerable<FinancialTransaction> transactions) :this()
        {
            Group = @group;
            Total = transactions.Sum(x => x.Amount);
        }
    }
}