using BankSystem.App;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using CsvHelper;
using CsvHelper.Configuration;
using ExportTool;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.DTOs;
using ExportTool.Maps;

namespace BankSystem.App.Tests
{
    public class ThreadAndTaskTests
    {
        private readonly object _locker = new();
        private Queue<Client> _clientQueue = new(); // Очередь для передачи клиентов между потоками
        private bool _producerFinished = false; // Флаг, показывающий, что генерация клиентов завершена
        private int _maxFileSizeBytes = 1000; // Максимальный размер файла в байтах
        private int _currentFileIndex = 0; // Индекс текущего файла


        [Fact]
        public void Pipeline_ShouldProcessClientsAndWriteMultipleFiles()
        {
            //Arrange

            // Создаём директорию и удаляем все ранее созданные тестовые файлы
            string basePath = Path.Combine("D:", "TestDIRECTORY");
            Directory.CreateDirectory(basePath);

            foreach (var file in Directory.GetFiles(basePath, "TestFile_*.csv"))
            {
                File.Delete(file);
            }

            TestDataGenerator generator = new TestDataGenerator();
            int clientsToGenerateCount = 1000;

            //Act

            // Поток-генератор клиентов: создаёт список клиентов и добавляет их в очередь
            Thread threadGenerator = new Thread(() =>
            {
                var generatedClients = generator.GenerateTestListClients(clientsToGenerateCount);
                lock (_locker)
                {
                    foreach (var client in generatedClients)
                    {
                        _clientQueue.Enqueue(client);
                    }

                    // Помечаем, что все клиенты сгенерированы
                    _producerFinished = true;
                }
            });

            // Поток-запись в CSV-файлы
            Thread threadWriter = new Thread(() =>
            {
                List<Client> buffer = new(); // Локальный буфер для накопления клиентов перед записью

                while (true)
                {
                    Client? client = null;

                    // Извлекаем клиента из очереди (если есть)
                    lock (_locker)
                    {
                        if (_clientQueue.Count > 0)
                        {
                            client = _clientQueue.Dequeue();
                        }
                        else if (_producerFinished && _clientQueue.Count == 0)
                        {
                            // Если очередь пуста и генерация завершена — записываем остаток буфера в последний файл
                            if (buffer.Count > 0)
                            {
                                string fileName = $"TestFile_{_currentFileIndex}.csv";

                                ExportService exportService = new ExportService(basePath, fileName);
                                exportService.ExportClientToCvsFile(buffer);
                                buffer.Clear();

                            }
                            break; // Завершаем поток записи
                        }
                    }


                    if (client != null)
                    {
                        buffer.Add(client);

                        string fileName = $"TestFile_{_currentFileIndex}.csv";
                        string fullPath = Path.Combine(basePath, fileName);

                        // Проверяем размер текущего файла, если превышает лимит — переходим к следующему
                        long currentFileSize = File.Exists(fullPath) ? new FileInfo(fullPath).Length : 0;

                        if (currentFileSize + 100 >= _maxFileSizeBytes)
                        {
                            _currentFileIndex++;

                        }
                        //Формируем имя актуального файла и записиваем буффер в него
                            string exactFileName = $"TestFile_{_currentFileIndex}.csv";

                            ExportService export = new ExportService(basePath, exactFileName);
                            export.ExportClientToCvsFile(buffer);

                            buffer.Clear();

                    }
                }
            });

            
            //Запуск потоков
            threadGenerator.Start();
            threadWriter.Start();
            threadGenerator.Join();
            threadWriter.Join();

            // Чтение всех сгенерированных файлов и сбор всех клиентов в один список
            List<Client> clientsFromCvs = new List<Client>();

            for (int i = 0; i < _currentFileIndex + 1; i++)
            {
                string fullpath = Path.Combine("D:", "TestDIRECTORY", $"TestFile_{i}.csv");

                using FileStream fileStream = new FileStream(fullpath, FileMode.Open);
                using StreamReader streamReader = new StreamReader(fileStream);

                CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                using CsvReader reader = new CsvReader(streamReader, config);
                reader.Context.RegisterClassMap<ClientCsvDtoMap>();

                var clientCsvDtos = reader.GetRecords<ClientCsvDto>().ToList();

                clientsFromCvs.AddRange(clientCsvDtos.Select(dto =>
                new Client(
                            dto.Id,
                            dto.FullName,
                            dto.Birthday,
                            dto.Email,
                            dto.PhoneNumber,
                            dto.PassportNumber
                            )));


            }

            // Assert — проверяем, что:
            // 1. Все клиенты были сохранены
            // 2. Нет дубликатов по Id
            Assert.Equal(clientsToGenerateCount, clientsFromCvs.Count);
            Assert.Equal(clientsFromCvs.Count, clientsFromCvs.Select(c => c.Id).Distinct().Count());
        }
    }
}


