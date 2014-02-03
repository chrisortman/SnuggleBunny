using System.IO;

namespace SnuggleBunny.Infrastructure
{
    public class FileDataSource : IFileDataSource
    {
        private readonly string _filename;

        public FileDataSource(string filename)
        {
            Guard.AgainstNull(filename,"filename");

            _filename = filename;
        }

        public bool Exists()
        {
            return File.Exists(_filename);
        }

        public ICsvReader ReadCsv()
        {
            return new CsvReader(new StreamReader(_filename));
        }

        public StreamReader ReadStream()
        {
            return new StreamReader(_filename);
        }
    }
}