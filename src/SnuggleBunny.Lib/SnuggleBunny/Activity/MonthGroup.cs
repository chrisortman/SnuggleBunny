using System;

namespace SnuggleBunny.Activity
{
    public struct MonthGroup : IEquatable<MonthGroup>
    {
        public MonthGroup(int year, int month)
            : this()
        {
            Year = year;
            Month = month;
        }

        public int Year { get; private set; }
        public int Month { get; private set; }

        public bool Equals(MonthGroup other)
        {
            return Year == other.Year && Month == other.Month;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MonthGroup && Equals((MonthGroup)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Year * 397) ^ Month;
            }
        }

        public static bool operator ==(MonthGroup left, MonthGroup right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MonthGroup left, MonthGroup right)
        {
            return !left.Equals(right);
        }
    }
}