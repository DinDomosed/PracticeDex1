using AutoMapper;
using BankSystem.App;
using BankSystem.App.Interfaces;
using BankSystem.App.Mappings;
using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain;
using BankSystem.Domain.Models;
using ExportTool;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class ExportServiceTests
    {
        [Fact]
        public void ExportToCvsFileTest()
        {
            //Arrange 
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ClientProfile>();
            configExpression.AddProfile<EmployeeProfile>();
            configExpression.AddProfile<AccountProfile>();

            var config = new MapperConfiguration(configExpression, new NullLoggerFactory());
            IMapper mapper = config.CreateMapper();

            BankSystemDbContext context = new BankSystemDbContext(options);
            ClientDbStorage storage = new ClientDbStorage(context);
            ClientService service = new ClientService(storage, mapper);
            
            TestDataGenerator generator = new TestDataGenerator();
            var clients = generator.GenerateTestListClients(10);

            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "SUPERTETS.csv";


            ExportService<Client> exportService = new ExportService<Client>(pathToDirectory, fileName, service);

            //Act 
            bool result = exportService.ExportClientToCvsFile(clients);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task ReadClientsFromCvsFileAndWriteToDb_Test()
        {
            //Arrange
            var configExpression = new MapperConfigurationExpression();

            configExpression.AddProfile<ClientProfile>();
            configExpression.AddProfile<EmployeeProfile>();
            configExpression.AddProfile<AccountProfile>();

            var config = new MapperConfiguration(configExpression, new NullLoggerFactory());
            IMapper mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase ("TestDb_cvs")
                .Options;

            BankSystemDbContext context = new BankSystemDbContext(options);
            IClientStorage clientStorage = new ClientDbStorage(context);
            ClientService clientService = new ClientService(clientStorage, mapper);

            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "SUPERTETS.csv";

            Directory.CreateDirectory(pathToDirectory);

            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName, clientService);
            string csvCntent = "ID;ФИО;Дата рождения;Почта;Номер телефона;Дата регистрации;Бонусы;Серия и номер паспорта\r\n" +
                "bc27dace-36f6-4ec4-a732-a6938409bdfe;Howard Breitenberg;04/13/1996 20:20:47;Zella_Labadie40@yahoo.com;+7 (163) 170-34-79;08/01/2025 22:02:20;;8838 972742\r\n21decbd4-ef10-4637-82fe-625c2fa7dc56;Frances Kulas;06/22/1933 01:13:49;Maurine_Schuppe@hotmail.com;+7 (376) 508-21-25;08/01/2025 22:02:20;;4813 788723\r\n" +
                "fe8296ec-d6d1-48ce-b4b1-462ecba14786;Catharine Hansen;08/24/1979 11:24:55;Fernando.Roberts@yahoo.com;+7 (217) 673-90-30;08/01/2025 22:02:20;;9788 728160\r\n";

            string fullpath = Path.Combine(pathToDirectory, fileName);
            File.WriteAllText(fullpath, csvCntent);

            //Act
            bool result = await export.ReadClientsFromCvsFileAndWriteToDbAsync();

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task ExportPersonToJsonFileAsync_Test()
        {
            //Arrange
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ClientProfile>();
            configExpression.AddProfile<EmployeeProfile>();
            configExpression.AddProfile<AccountProfile>();

            var config = new MapperConfiguration(configExpression, new NullLoggerFactory());
            IMapper mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            BankSystemDbContext context = new BankSystemDbContext(options);
            IClientStorage clientStorage = new ClientDbStorage(context);
            ClientService clientService = new ClientService(clientStorage, mapper);

            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "JsonSuperTets_1.json";
            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName, clientService);

            TestDataGenerator generator = new TestDataGenerator();
            var client = new Client("Тестовый клиент 1101", new DateTime(2000, 12, 20), "test1212@mail.ru", "+79182342323", "1234 567890");

            client.Accounts.Add(new Account(client.Id, new Currency("USD", '$'), 1000));


            //Act

            bool result = await export.ExportPersonToJsonFileAsync(client);

            //Assert
            Assert.True(result);

        }
        [Fact]
        public async Task ImportPersonFromJsonfileAsync()
        {
            //Arrange
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ClientProfile>();
            configExpression.AddProfile<EmployeeProfile>();
            configExpression.AddProfile<AccountProfile>();

            var config = new MapperConfiguration(configExpression, new NullLoggerFactory());
            IMapper mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            BankSystemDbContext context = new BankSystemDbContext(options);
            IClientStorage clientStorage = new ClientDbStorage(context);
            ClientService clientService = new ClientService(clientStorage, mapper);

            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "JsonSuperTets_1.json";
            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName, clientService);

            //Act
            var result = await export.ImportPersonFromJsonfileAsync();

            //Assert
            Assert.Equal(1000, result.Accounts.First().Amount);
            Assert.Equal("Тестовый клиент 1101", result.FullName);

        }

        [Fact]
        public async Task ExportListPersonsToJsonFileAsync_Test()
        {
            //Arrange
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ClientProfile>();
            configExpression.AddProfile<EmployeeProfile>();
            configExpression.AddProfile<AccountProfile>();

            var config = new MapperConfiguration(configExpression, new NullLoggerFactory());
            IMapper mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            BankSystemDbContext context = new BankSystemDbContext(options);
            IClientStorage clientStorage = new ClientDbStorage(context);
            ClientService clientService = new ClientService(clientStorage, mapper);

            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "JsonSuperTets_2.json";
            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName, clientService);

            TestDataGenerator generator = new TestDataGenerator();
            var clients = generator.GenerateTestListClients(10);

            foreach (var client in clients)
            {
                client.Accounts.Add(new Account(client.Id, new Currency("USD", '$'), 1000));
            }

            //Act

            bool result = await export.ExportListPersonsToJsonFileAsync(clients);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task ImportListPersonsFromJsonfileAsync_Test()
        {
            //Arrange 
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ClientProfile>();
            configExpression.AddProfile<EmployeeProfile>();
            configExpression.AddProfile<AccountProfile>();

            var config = new MapperConfiguration(configExpression, new NullLoggerFactory());
            IMapper mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            BankSystemDbContext context = new BankSystemDbContext(options);
            IClientStorage clientStorage = new ClientDbStorage(context);
            ClientService clientService = new ClientService(clientStorage, mapper);


            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "JsonSuperTets_2.json";
            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName, clientService);

            //Act
            var importedClients = await export.ImportListPersonsFromJsonfileAsync();

            //Assert
            Assert.Equal(10, importedClients.Count);
            Assert.NotNull(importedClients);
            Assert.NotNull(importedClients.First());
        }
    }
}
