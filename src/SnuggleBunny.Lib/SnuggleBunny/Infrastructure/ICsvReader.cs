using System;

namespace SnuggleBunny.Infrastructure
{
    public interface ICsvReader : IDisposable
    {
        string this[int fieldIndex] { get; }
        bool EOF { get; }
        bool Read();
    }
}