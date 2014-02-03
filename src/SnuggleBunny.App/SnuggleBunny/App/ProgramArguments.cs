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
namespace SnuggleBunny.App
{
    using System;
    using System.IO;

    /// <summary>
    /// Represents command line arguments to the program
    /// </summary>
    public class ProgramArguments
    {
        /// <summary>
        /// The config file to use
        /// </summary>
        public string ConfigFile { get; private set; }
        
        /// <summary>
        /// The transaction file to use
        /// </summary>
        public string TransactionFile { get; private set; }

        /// <summary>
        /// Determines if the arguments to the program are valid
        /// </summary>
        public bool IsValid { get; private set; }

        public ProgramArguments(string[] commandLine)
        {
            string[] commandLineArgs = commandLine;
            if (commandLineArgs.Length == 2)
            {
                ConfigFile = commandLineArgs[0];
                TransactionFile = commandLineArgs[1];
                IsValid = true;
            }
        }

        /// <summary>
        /// Verifies that the files exist.
        /// </summary>
        /// <returns></returns>
        public bool VerifyFiles()
        {
            if (!File.Exists(ConfigFile))
            {
                Console.WriteLine("Config file {0} does not exist", Path.GetFullPath(ConfigFile));
                return false;
            }

            if (!File.Exists(TransactionFile))
            {
                Console.WriteLine("Transaction file {0} does not exist", Path.GetFullPath(TransactionFile));
                return false;
            }
            return true;
        }
    }
}