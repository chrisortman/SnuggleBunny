using System.Linq;

namespace SnuggleBunny.Activity
{
    public static class By
    {
        public static MonthGroup Month(FinancialTransaction transaction)
        {
            return new MonthGroup(transaction.OccurredOn.Year, transaction.OccurredOn.Month);
        }

        public static MonthCategoryGroup MonthAndCategory(FinancialTransaction transaction)
        {
            return new MonthCategoryGroup(transaction.OccurredOn.Month, transaction.OccurredOn.Year,
                transaction.Category);
        }
    }

    public static class Transactions
    {
        public static TotalledTransactionGroup<TGroup> Totalled<TGroup>(IGrouping<TGroup, FinancialTransaction> transactionGroup)
        {
            return new TotalledTransactionGroup<TGroup>(transactionGroup.Key, transactionGroup);
        }
    }
}