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
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [DebuggerStepThrough]
    public static class Guard
    {
        /// <summary>
        ///     Prevents the given assertion from ocurring
        /// </summary>
        /// <param name="assertion"></param>
        /// <param name="message"></param>
        [AssertionMethod]
        public static void Against([AssertionCondition(AssertionConditionType.IS_FALSE)] bool assertion, string message)
        {
            if (assertion)
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        ///     Prevents the given assertion from ocurring
        /// </summary>
        /// <typeparam name="EX">Type of exception to raise on error</typeparam>
        /// <param name="assertion"></param>
        /// <param name="message"></param>
        [AssertionMethod]
        public static void Against<EX>([AssertionCondition(AssertionConditionType.IS_FALSE)] bool assertion,
            string message) where EX : Exception
        {
            if (assertion)
            {
                throw (EX) Activator.CreateInstance(typeof (EX), message);
            }
        }

        /// <summary>
        ///     Verifies that the given ondition is true
        /// </summary>
        /// <typeparam name="EX"></typeparam>
        /// <param name="assertion"></param>
        /// <param name="message">If EX is ArgumentNullException will be param name</param>
        [AssertionMethod]
        public static void Requires<EX>([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
            string message) where EX : Exception
        {
            if (!assertion)
            {
                throw (EX) Activator.CreateInstance(typeof (EX), message);
            }
        }

        /// <summary>
        ///     Verifies that the given ondition is true
        /// </summary>
        /// <typeparam name="EX"></typeparam>
        /// <param name="assertion"></param>
        /// <param name="message">If EX is ArgumentNullException will be param name</param>
        [AssertionMethod]
        public static void Requires([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message)
        {
            if (!assertion)
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        ///     Verifies the given object is not null
        /// </summary>
        /// <param name="o"></param>
        /// <param name="paramName"></param>
        /// <param name="message"></param>
        [AssertionMethod]
        public static void AgainstNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] object o,
            [InvokerParameterName] string paramName, string message = null)
        {
            if (o == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }
    }


}