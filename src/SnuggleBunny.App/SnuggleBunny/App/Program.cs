using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnuggleBunny.App
{
    using System.IO;
    using Activity;
    using Budget.Analyzers;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: snugglebunny.app.exe <config_file> <transaction_file>");
                return;
            }

            var configFile = args[0];
            var transactionFile = args[1];

            if (!File.Exists(configFile))
            {
                Console.WriteLine("Config file {0} does not exist",Path.GetFullPath(configFile));
                return;
            }

            if (!File.Exists(transactionFile))
            {
                Console.WriteLine("Transaction file {0} does not exist", Path.GetFullPath(transactionFile));
            }

            var tool = new BudgetTool(configFile);
            tool.AddAnalyzer(new CategorySpendingLimitAnalyzer());
            tool.AddAnalyzer(new MonthlySpendingVersusIncomeAnalyzer());

            var report = new ActivityReport();
            report.Load(transactionFile);

            var alerts = tool.Analyze(report);
            foreach (var alert in alerts)
            {
                Console.WriteLine(alert.Describe());
            }

        }
    }
}
