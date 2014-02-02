using System;

namespace SnuggleBunny.Activity
{
    public class FinancialTransactionCsvParser
    {
        public bool TryParseLine(string line,out FinancialTransaction transaction)
        {
            Guard.AgainstNull(line, "line");

            var parts = line.Split(',');
            if (parts.Length != 4)
            {
                transaction = null;
                return false;
            }

            transaction = new FinancialTransaction();
            transaction.Description = parts[1];
            transaction.Category = parts[3];

            DateTime dt;
            if (DateTime.TryParse(parts[0],out dt))
            {
                transaction.OccurredOn = dt;
            }
            else
            {
                ErrorMessage = String.Format("Invalid date format '{0}'", parts[0]);
                return false;
            }

            Decimal d;
            if (Decimal.TryParse(parts[2], out d))
            {
                transaction.Amount = d;
            }
            else
            {
                ErrorMessage = String.Format("Invalid money format '{0}'", parts[2]);
                return false;
            }

            return true;
        }

        public string ErrorMessage { get; private set; }
    }
}