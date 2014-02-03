#region LICENSE

// The MIT License (MIT)
// 
// Copyright (c) <year> <copyright holders>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

namespace SnuggleBunny
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///     Encapsulates optional references. Prevent NULL refrence exceptions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExcludeFromCodeCoverage]
    public struct Maybe<T>
    {
        public static readonly Maybe<T> Nothing = new Maybe<T>();
        private readonly bool _hasValue;
        private readonly T _value;

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