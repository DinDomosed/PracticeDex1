using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Bogus;

namespace BankSystem.App.Tests
{

    public class ClientStorageTests
    {
        [Fact]
        public async Task AddClientToStorageTest_Count_10()
        {
            //Arrange
            IClientStorage ClientStorage = new ClientStorage(); 
            TestDataGenerator generator = new TestDataGenerator();

            //Act
            var testListClient = generator.GenerateTestListClients(10);
            foreach (var client in testListClient)
            {
                await ClientStorage.AddAsync(client);
            }

            var result = await ClientStorage.GetAllAsync();

            //Assert 
            Assert.Equal(10, result.Count);
        }

        [Fact]
        public async Task GetYoungestClientFromStorageClient()
        {
            //Arrange
            IClientStorage ClientStorage = new ClientStorage();
            TestDataGenerator generator = new TestDataGenerator();

            Client clientTest = new Client("Тестовый самый молодой клиент", new DateTime(2020, 11, 2), "Clava007@mail.ru", "+7 918 123 36 78", "4324 964623");
            await ClientStorage.AddAsync(clientTest);

            var testGenerateClients = generator.GenerateTestListClients(10);
            foreach (var client in testGenerateClients)
            {
                await ClientStorage.AddAsync(client);
            }


            //Act
            var allClients = await ClientStorage.GetAllAsync();
            var youngestClient = allClients.OrderBy(u => u.Birthday).LastOrDefault();

            bool resultEqualBirthday = youngestClient.Birthday.Equals(new DateTime(2020, 11, 2));
            bool resultEqualFullName = youngestClient.FullName.Equals("Тестовый самый молодой клиент");

            //Assert
            Assert.NotNull(youngestClient);
            Assert.True(resultEqualBirthday);
            Assert.True(resultEqualFullName);
        }

        [Fact]
        public async Task GetOldestClientFromStorageClient()
        {
            //Arrange
            IClientStorage ClientStorage = new ClientStorage();
            TestDataGenerator generator = new TestDataGenerator();

            await ClientStorage.AddAsync(new Client("Тестовый клиент", new DateTime(1950, 11, 2), "Clava007@mail.ru", "+7 918 123 36 78", "4324 964623"));

            var testGenerateClients = generator.GenerateTestListClients(10);
            //Act
            var allClients = await ClientStorage.GetAllAsync();
            var olderClient = allClients.OrderBy(u => u.Birthday).First();
            bool resultEqualBirthday = olderClient.Birthday.Equals(new DateTime(1950, 11, 2));
            bool resultEqualFullName = olderClient.FullName.Equals("Тестовый клиент");

            //Assert
            Assert.True(resultEqualFullName);
            Assert.True(resultEqualBirthday);
            Assert.NotNull(olderClient);
        }

        [Fact]
        public async Task GetAverageAgeClient()
        {
            //Arrange
            List<Client> clientList = new List<Client>(); //10
            clientList.AddRange(new Client[]
            {
                new Client("Тестовый клиент1", new DateTime(2006, 12, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111"), //18

                new Client("Тестовый клиент2", new DateTime(2000, 12, 2), "testClient2.ru", "+7 918 222 22 22", "4324 222222"), //24

                new Client("Тестовый клиент3", new DateTime(2010, 12, 31), "testClient3.ru", "+7 918 333 33 33", "4324 333333"),  //14

                new Client("Тестовый клиент4", new DateTime(2009, 12, 24), "testClient4.ru", "+7 918 444 44 44", "4324 444444"), //15

                new Client("Тестовый клиент5", new DateTime(2003, 12, 8), "testClient5.ru", "+7 918 555 55 55", "4324 555555"), //21

                new Client("Тестовый клиент6", new DateTime(2002, 12, 6), "testClient6.ru", "+7 918 666 66 66", "4324 666666"), //22

                new Client("Тестовый клиент7", new DateTime(2001, 12, 21), "testClient7.ru", "+7 918 777 77 77", "4324 777777"), //23

                new Client("Тестовый клиент8", new DateTime(2002, 12, 4), "testClient8.ru", "+7 918 888 88 88", "4324 888888"), //22

                new Client("Тестовый клиент9", new DateTime(2000, 12, 2), "testClient9.ru", "+7 918 999 99 99", "4324 999999"), //24

                new Client("Тестовый клиент10", new DateTime(1980, 12, 17), "testClient10.ru", "+7 918 000 00 00", "4324 000000"), //44
        });

            IClientStorage ClientStorage = new ClientStorage();
            foreach (var client in clientList)
            {
                await ClientStorage.AddAsync(client);
            }

            //Act
            var allClients = await ClientStorage.GetAllAsync();
            int sumAge = allClients.Sum(c => c.Age);
            int result = sumAge / allClients.Count; //22

            int expectedResult = 22;
            DateTime originalDate = new DateTime(2025, 8, 27);
            //Подскатраиваю ожидаемый результат с учетом течения времени 
            if(DateTime.Now.Year > originalDate.Year)
            {
                int addToExpectedResult = DateTime.Now.Year - originalDate.Year;
                expectedResult += addToExpectedResult;
            }

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
