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
            var programArgs = new ProgramArguments(args);
            if (programArgs.IsValid)
            {
                if (programArgs.VerifyFiles())
                {
                    var alerts = RunTool(programArgs);
                    PrintAlerts(alerts);
                }
            }
            else
            {
                PrintUsage();
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: snugglebunny.app.exe <config_file> <transaction_file>");
        }

        private static IReadOnlyCollection<ISpendingAlert> RunTool(ProgramArguments programArgs)
        {
            var tool = new BudgetTool(programArgs.ConfigFile);
            tool.AddAnalyzer(new CategorySpendingLimitAnalyzer());
            tool.AddAnalyzer(new MonthlySpendingVersusIncomeAnalyzer());

            var report = new ActivityReport();
            report.Load(programArgs.TransactionFile);

            var alerts = tool.Analyze(report);
            return alerts;
        }

        private static void PrintAlerts(IReadOnlyCollection<ISpendingAlert> alerts)
        {
            foreach (var alert in alerts)
            {
                Console.WriteLine(alert.Describe());
            }
        }
    }
}
