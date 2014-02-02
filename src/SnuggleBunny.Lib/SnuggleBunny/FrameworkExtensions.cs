using System;

namespace SnuggleBunny
{
    public static class FrameworkExtensions
    {
        /// <summary>
        /// Checks to see if a string is null or ""
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsBlank(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }
    }
}