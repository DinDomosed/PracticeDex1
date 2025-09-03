using BankSystem.App;
using BankSystem.App.DTOs;
using BankSystem.App.Exceptions;
using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain;
using BankSystem.Domain.Models;
using CsvHelper;
using CsvHelper.Configuration;
using ExportTool.Maps;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text;


namespace ExportTool
{
    public class ExportService
    {
        protected string PathToDirectory { get; private set; }
        protected string FileName { get; private set; }

        public ExportService(string pathToDirectory, string fileName)
        {
            PathToDirectory = pathToDirectory;
            FileName = fileName;
        }
        public bool ExportClientToCvsFile(List<Client> data)
        {
            if (data == null || data.Count == 0)
                return false;

            DirectoryInfo dirInfo = new DirectoryInfo(PathToDirectory);

            if (!dirInfo.Exists)
                dirInfo.Create();

            string fullpath = Path.Combine(PathToDirectory, FileName);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = !File.Exists(fullpath)
            };

            List<ClientCsvDto> dtoData = data.Select(c => new ClientCsvDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Birthday = c.Birthday,
                Bonus = c.Bonus,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                PassportNumber = c.PassportNumber,
                RegistrationDate = c.RegistrationDate

            }).ToList();

            FileMode mode = (File.Exists(fullpath)) ? FileMode.Append : FileMode.Create;

            using (FileStream fileStream = new FileStream(fullpath, mode))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, new UTF8Encoding(true)))
                {
                    using (CsvWriter writer = new CsvWriter(streamWriter, config))
                    {
                        writer.Context.RegisterClassMap<ClientCsvDtoMap>();

                        writer.WriteRecords(dtoData);
                        return true;
                    }
                }
            }
        }
        public async Task<bool> ReadClientsFromCvsFileAndWriteToDbAsync()
        {
            DirectoryInfo directory = new DirectoryInfo(PathToDirectory);

            if (!directory.Exists)
                return false;

            string fullpath = Path.Combine(PathToDirectory, FileName);
            try
            {
                using (FileStream fileStream = new FileStream(fullpath, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            Delimiter = ";",
                            HeaderValidated = null
                        };
                        using (CsvReader reader = new CsvReader(streamReader, config))
                        {
                            reader.Context.RegisterClassMap<ClientCsvDtoMap>();
                            var recordsDto = reader.GetRecords<ClientCsvDto>();

                            List<Client> records = recordsDto.Select(dto =>
                            new Client(
                                dto.Id,
                                dto.FullName,
                                dto.Birthday,
                                dto.Email,
                                dto.PhoneNumber,
                                dto.PassportNumber
                                )).ToList();

                            using (BankSystemDbContext dbContext = new BankSystemDbContext())
                            {
                                ClientDbStorage dbStorage = new ClientDbStorage(dbContext);
                                ClientService service = new ClientService(dbStorage);

                                foreach (var record in records)
                                {
                                    try
                                    {
                                        await service.AddClientAsync(record);
                                    }
                                    catch (PassportNumberNullOrWhiteSpaceException passportEx)
                                    {
                                        continue;
                                    }
                                    catch (InvalidClientAgeException InvalidAgeEx)
                                    {
                                        continue;
                                    }
                                    catch (ArgumentNullException ArgNullEx)
                                    {
                                        continue;
                                    }
                                    catch (ArgumentException argEx)
                                    {
                                        continue;
                                    }
                                }
                                return true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
