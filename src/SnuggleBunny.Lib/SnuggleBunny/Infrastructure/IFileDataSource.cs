using System.IO;

namespace SnuggleBunny.Infrastructure
{
    public interface IFileDataSource
    {
        bool Exists();
        ICsvReader ReadCsv();
        StreamReader ReadStream();
    }
}