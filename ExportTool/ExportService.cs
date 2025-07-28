using BankSystem.Domain;
using BankSystem.Domain.Models;
using System.Text;
using CsvHelper;
using System.Globalization;

namespace ExportTool
{
    public class ExportService<T>
    {
        protected string PathToDirectory { get; private set; }
        protected string FileName { get; private set; }

        public ExportService(string pathToDirectory, string fileName)
        {
            PathToDirectory = pathToDirectory;
            FileName = fileName;
        }
        public void ExportToCvsFile(List<T> data)
        {
            if (data.Count == 0 || data == null)
                return;

            DirectoryInfo dirInfo = new DirectoryInfo(PathToDirectory);

            if (!dirInfo.Exists)
                dirInfo.Create();

            string fullpath = Path.Combine(PathToDirectory, FileName);

            bool FileNeedsHeaders = !File.Exists(fullpath) || new FileInfo(fullpath).Length == 0;

            using (FileStream fileStream = new FileStream(fullpath, FileMode.OpenOrCreate))
            {
                using(StreamWriter streamWriter = new StreamWriter(fileStream, new UTF8Encoding()))
                {
                    using (CsvWriter writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        if(FileNeedsHeaders)
                        {
                            writer.WriteHeader<Client>();
                            writer.NextRecord();
                        }

                        foreach(var client in data)
                        {
                            writer.WriteRecord(client);
                            writer.NextRecord();
                        }

                        writer.Flush();
                    }
                }
            }
        }
    }
}
