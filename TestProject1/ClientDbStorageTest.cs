using BankSystem.App.DTOs;
using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class ClientDbStorageTest
    {
        [Fact]
        public void Add_Test()
        {
            //Arrange 
            BankSystemDbContext dbContext = new BankSystemDbContext();
            ClientDbStorage dbStorageClient = new ClientDbStorage(dbContext);
            TestDataGenerator generator = new TestDataGenerator();

            List<Client> clients = generator.GenerateTestListClients(10);

            //Act
            bool result = false;
            foreach (var client in clients)
            {
                result = dbStorageClient.Add(client);
            }
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void Update_Test()
        {
            //Arrange
            BankSystemDbContext dbcontext = new BankSystemDbContext();
            ClientDbStorage dbStorageClient = new ClientDbStorage(dbcontext);

            //Act
            Client newDataClient = new Client(Guid.Parse("43f12e09-d109-4f33-ba54-4d16884ff3e6"), "Супер-Ультра ITшник", new DateTime(2000, 9, 6), "tiktik@mail.ru",
                "+7 918 111 11 11", "4324 111111");
            bool result = dbStorageClient.Update(Guid.Parse("43f12e09-d109-4f33-ba54-4d16884ff3e6"), newDataClient);

            //Assert
            Assert.True(result);
            Client dbClient = dbStorageClient.Get(Guid.Parse("43f12e09-d109-4f33-ba54-4d16884ff3e6"));
            Assert.Equal("+7 918 111 11 11", dbClient.PhoneNumber);
            Assert.Equal("Супер-Ультра ITшник", dbClient.FullName);
            Assert.NotEqual("Тестовый клиент1", dbClient.FullName);
            Assert.NotEqual("testClient1.ru", dbClient.Email);
        }
        [Fact]
        public void Delete_test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            ClientDbStorage dbStorage = new ClientDbStorage(dbContext);


            Client client = new Client(Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"), "Тестовый клиент3", new DateTime(2003, 12, 31), "testClient3.ru", "+7 918 333 33 33", "4324 333333");
            dbStorage.Add(client);

            //Act
            bool result = dbStorage.Delete(Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"));

            using var dbContext2 = new BankSystemDbContext();
            var dbStorage2 = new ClientDbStorage(dbContext2);
            var notfoundClient = dbStorage2.Get(Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"));

            //Assert

            Assert.Null(notfoundClient);
            Assert.True(result);
        }

        [Fact]
        public void AddAccount_Test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            ClientDbStorage dbStorage = new ClientDbStorage(dbContext);

            Account newAccount = new Account(Guid.Parse("13a82352-10dd-4f0b-bdc3-c5b0c0c52daf"), new Currency("RUB", '₽'), 6000);
            //Act
            bool result = dbStorage.AddAccount(Guid.Parse("13a82352-10dd-4f0b-bdc3-c5b0c0c52daf"), newAccount);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateAccount_Test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            ClientDbStorage dbStorage = new ClientDbStorage(dbContext);

            var foundClient = dbStorage.Get(Guid.Parse("4c9b91bf-281e-4fbd-8973-0f04d1746774"));
            var oldAccount = foundClient.Accounts.FirstOrDefault(c => c.AccountNumber == "4081193022794");
            bool result1 = false;


            Account newAccount = new Account(foundClient.Id, new Currency("RUB", '₽'), 6000);
            //Act
            bool result2 = dbStorage.UpdateAccount(foundClient.Id, oldAccount.AccountNumber, newAccount);
            var foundClient2 = dbStorage.Get(Guid.Parse("4c9b91bf-281e-4fbd-8973-0f04d1746774"));

            //Assert
            Assert.True(result2);
            Assert.Equal("RUB", foundClient2.Accounts.FirstOrDefault(c => c.AccountNumber == "4081193022794").CurrencyCode);
            
        }

        [Fact]
        public void DeleteAccount_test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            ClientDbStorage dbStorage = new ClientDbStorage(dbContext);

            //Act
            Account newAccount = new Account(Guid.Parse("ab9089a8-4484-4bf9-8264-b3d83bf4caaa"), new Currency("USD", '$'), 6000);
            bool result1 = dbStorage.AddAccount(Guid.Parse("ab9089a8-4484-4bf9-8264-b3d83bf4caaa"), newAccount);
            bool result = dbStorage.DeleteAccount(Guid.Parse("ab9089a8-4484-4bf9-8264-b3d83bf4caaa"), newAccount.AccountNumber);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetFilterClients_Test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            ClientDbStorage dbStorage = new ClientDbStorage(dbContext);
            ClientFilterDTO filter = new ClientFilterDTO
            {
                CountAccounts = 2
            };

            //Act
            var result = dbStorage.GetFilterClients(filter, 1);

            //Assert
            Assert.Equal(4, result.Items.Count());
        }
        [Fact]
        public void Exists_Test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            ClientDbStorage dbStorage = new ClientDbStorage(dbContext);

            //Act
            bool result = dbStorage.Exists(Guid.Parse("8a5d71f1-8566-4b11-8917-fdcc44a4e8a1"), "4884 630650");

            //Assert
            Assert.True(result);
        }
    }
}
