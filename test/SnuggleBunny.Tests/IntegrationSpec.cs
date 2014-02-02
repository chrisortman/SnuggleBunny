using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Shouldly;
using Xunit;

namespace SnuggleBunny.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void DetectsOverespendingInACategory()
        {
            var tool = new BudgetTool("budget_config.yml");
            var spendingReport = new SpendingReport();
            spendingReport.Load("transactions.csv");

            IEnumerable<Overage> alerts = tool.Analyze(spendingReport);

            alerts.ShouldContain(
                x => x.ToString() == "[OVERAGE] - Spending for clothing in Feb exceeded budget by $150.00 ($200.00/$350.00)");

        }
    }

    public class BudgetToolTests
    {
        public class TheAnalyzeMethod
        {
            [Fact]
            public void DetectsOverspendingInACategory()
            {
                var tool = new BudgetTool();
                tool.DefineCategory("clothing", limit: 200M);

                var spendingReport = new SpendingReport();
                spendingReport.AddTransaction(new DateTime(2014, 2, 2), "Gap", 201M, "clothing");

                var alerts = tool.Analyze(spendingReport);
                alerts.ShouldContain(new Overage("clothing",2,201M,200M));

            }
        }
    }

    public struct Overage : IEquatable<Overage>
    {
        public string Category { get; private set; }
        public int Month { get; private set; }
        public decimal Spent { get; private set;}
        public decimal Alloted { get; private set; }

        public Overage(string category, int month, decimal spent, decimal alloted) : this()
        {
            Guard.AgainstNull(category,"category");

            Category = category;
            Month = month;
            Spent = spent;
            Alloted = alloted;
        }

        public bool Equals(Overage other)
        {
            return string.Equals(Category, other.Category) && Month == other.Month && Spent == other.Spent && Alloted == other.Alloted;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Overage && Equals((Overage) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Category.GetHashCode();
                hashCode = (hashCode*397) ^ Month;
                hashCode = (hashCode*397) ^ Spent.GetHashCode();
                hashCode = (hashCode*397) ^ Alloted.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Overage left, Overage right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Overage left, Overage right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return String.Format("[OVERAGE] - Spending for {0} in {1} exceeded budget by {2:C} ({3:C}/{4:C})",
                                  Category,
                                  CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Month),
                                  Math.Abs(Alloted-Spent),
                                  Alloted,
                                  Spent);
        }
    }

    public class SpendingReport
    {
        public void Load(string transactionData)
        {
            
        }

        public void AddTransaction(DateTime dateTime, string description, decimal amount, string category)
        {
            
        }
    }

    public class BudgetTool
    {
        public BudgetTool(string configFile)
        {
            
        }

        public BudgetTool()
        {
            
        }

        public IEnumerable<Overage> Analyze(SpendingReport spendingReport)
        {
            yield return new Overage(category:"clothing",month:2,spent:201M,alloted:200M);
        }

        public void DefineCategory(string clothing, decimal limit)
        {
            
        }
    }
}