namespace SnuggleBunny.Infrastructure
{
    public interface ICsvReader
    {
        string this[int fieldIndex] { get; }
        bool EOF { get; }
        bool Read();
    }
}