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
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;


namespace ExportTool
{
    public class ExportService<T> where T : Person

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

        public async Task<bool> ExportPersonToJsonFileAsync(T person)
        {
            if (person == null)
                return false;

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                {
                    DefaultMembersSearchFlags =
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance
                },
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Ignore
                
            };

            string jsonPerson = JsonConvert.SerializeObject(person, settings);

            string fullPath = Path.Combine(PathToDirectory, FileName);

            await File.WriteAllTextAsync(fullPath, jsonPerson);

            return true;
        }
        public async Task<bool> ExportListPersonsToJsonFileAsync(List<T> persons)
        {
            if (persons == null)
                throw new ArgumentNullException(nameof(persons));

            string fullPath = Path.Combine(PathToDirectory, FileName);

            using FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, useAsync:true);
            using StreamWriter streamWriter = new StreamWriter(fileStream);
            using (JsonTextWriter writer = new JsonTextWriter(streamWriter))
            {
                JsonSerializer jsonSerializer = new JsonSerializer
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                    {
                        DefaultMembersSearchFlags =
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance
                    },
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.All,
                    NullValueHandling = NullValueHandling.Ignore
                };

                await Task.Run(() => jsonSerializer.Serialize(writer, persons));
                await streamWriter.FlushAsync();
            }
            return true;
        }
        public async Task<T> ImportPersonFromJsonfileAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathToDirectory);

            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException($"Директория '{PathToDirectory}' не найдена.");


            string fullPath = Path.Combine(PathToDirectory, FileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Файл '{FileName}' не найден по пути '{fullPath}'.");


            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                {
                    DefaultMembersSearchFlags  =
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance
                },
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
            };

            var readData = await File.ReadAllTextAsync(fullPath);
            var deserializedPerson = JsonConvert.DeserializeObject<T>(readData, setting);

            return deserializedPerson;
        }
        public async Task<List<T>> ImportListPersonsFromJsonfileAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathToDirectory);
            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException($"Директория '{PathToDirectory}' не найдена.");

            string fullPath = Path.Combine(PathToDirectory, FileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Файл '{FileName}' не найден по пути '{fullPath}'.");

            
            using FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new StreamReader(fileStream);
            using(JsonTextReader  reader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                    {
                        DefaultMembersSearchFlags =
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance
                    },
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.All
                };

                return await Task.Run(() => serializer.Deserialize<List<T>>(reader));
            }
        }
    }
}
