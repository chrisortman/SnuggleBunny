using System.Diagnostics.CodeAnalysis;

namespace SnuggleBunny
{
    /// <summary>
    /// Encapsulates optional references. Prevent NULL refrence exceptions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExcludeFromCodeCoverage]
    public struct Maybe<T>
    {
        public readonly static Maybe<T> Nothing = new Maybe<T>();
        private readonly T _value;
        private readonly bool _hasValue;

        public Maybe(T value)
        {
            _value = value;
            _hasValue = value == null ? false : true;
        }

        public T Value
        {
            get { return _value; }
        }

        public bool HasValue
        {
            get { return _hasValue; }
        }

        public override string ToString()
        {
            if (!HasValue)
            {
                return "<Nothing>";
            }

            return Value.ToString();
        }
    }

    [ExcludeFromCodeCoverage]
    public static class Maybe
    {
        public static bool IsSomething<T>(this Maybe<T> a)
        {
            return a.HasValue;
        }

        public static bool IsNothing<T>(this Maybe<T> a)
        {
            return !a.IsSomething();
        }

        public static Maybe<T> ToMaybe<T>(this T a)
        {
            if (a == null)
            {
                return Maybe<T>.Nothing;
            }
            return new Maybe<T>(a);
        }
    }
}