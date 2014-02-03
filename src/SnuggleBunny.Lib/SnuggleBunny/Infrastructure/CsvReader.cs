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

namespace SnuggleBunny.Infrastructure
{
    using System;
    using System.IO;

    public sealed class CsvReader : ICsvReader
    {
        private readonly TextReader _innerReader;
        private string _currentLine;
        private string[] _currentLineParts;

        public CsvReader(TextReader stringReader)
        {
            _innerReader = stringReader;
        }

        public string this[int fieldIndex]
        {
            get
            {
                EnsureReadData();
                return _currentLineParts[fieldIndex];
            }
        }

        public bool EOF
        {
            get { return _innerReader.Peek() == -1; }
        }

        public bool Read()
        {
            string line = null;
            while (line.IsBlank() && !EOF)
            {
                line = _innerReader.ReadLine();
            }
            _currentLine = line;

            if (!_currentLine.IsBlank())
            {
                _currentLineParts = _currentLine.Split(',');
            }
            return _currentLine != null;
        }


        public void Dispose()
        {
            if (_innerReader != null)
            {
                _innerReader.Dispose();
            }
        }

        private void EnsureReadData()
        {
            if (_currentLine == null)
            {
                throw new InvalidOperationException("You must read before accessing any fields");
            }
        }
    }
}