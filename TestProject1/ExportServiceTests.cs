using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportTool;
using BankSystem.Domain;
using BankSystem.Domain.Models;
using BankSystem.App;
using BankSystem.App.Services;

namespace BankSystem.App.Tests
{
    public class ExportServiceTests
    {
        [Fact]
        public void ExportToCvsFileTest()
        {
            //Arrange 

            TestDataGenerator generator = new TestDataGenerator();
            var clients = generator.GenerateTestListClients(10);

            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "SUPERTETS.csv";


            ExportService<Client> exportService = new ExportService<Client>(pathToDirectory, fileName);

            //Act 
            bool result = exportService.ExportClientToCvsFile(clients);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task ReadClientsFromCvsFileAndWriteToDb_Test()
        {
            //Arrange
            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "SUPERTETS.csv";
            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName);

            //Act
            bool result = await export.ReadClientsFromCvsFileAndWriteToDbAsync();

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task ExportPersonToJsonFileAsync_Test()
        {
            //Arrange
            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "JsonSuperTets_1.json";
            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName);

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
            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "JsonSuperTets_1.json";
            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName);

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
            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "JsonSuperTets_2.json";
            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName);

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
            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "JsonSuperTets_2.json";
            ExportService<Client> export = new ExportService<Client>(pathToDirectory, fileName);

            //Act
            var importedClients = await export.ImportListPersonsFromJsonfileAsync();

            //Assert
            Assert.Equal(10, importedClients.Count);
            Assert.NotNull(importedClients);
            Assert.NotNull(importedClients.First());
        }
    }
}
