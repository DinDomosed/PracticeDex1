using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.DTOs;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class ClientServiceTests
    {
        [Fact]
        public void AddClient_Count_10()
        {
            //Arrange
            IClientStorage FakeClientStorage = new FakeClientStorage();
            ClientService clientService = new ClientService(FakeClientStorage);

            List<Client> clientList = new List<Client>(); //10
            clientList.AddRange(new Client[]
            {
                new Client("Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111"),

                new Client("Тестовый клиент2", new DateTime(2001, 11, 2), "testClient2.ru", "+7 918 222 22 22", "4324 222222"),

                new Client("Тестовый клиент3", new DateTime(2002, 12, 31), "testClient3.ru", "+7 918 333 33 33", "4324 333333"),

                new Client("Тестовый клиент4", new DateTime(1999, 10, 24), "testClient4.ru", "+7 918 444 44 44", "4324 444444"),

                new Client("Тестовый клиент5", new DateTime(2003, 8, 8), "testClient5.ru", "+7 918 555 55 55", "4324 555555"),

                new Client("Тестовый клиент6", new DateTime(2002, 7, 6), "testClient6.ru", "+7 918 666 66 66", "4324 666666"),

                new Client("Тестовый клиент7", new DateTime(2001, 8, 21), "testClient7.ru", "+7 918 777 77 77", "4324 777777"),

                new Client("Тестовый клиент8", new DateTime(2002, 10, 4), "testClient8.ru", "+7 918 888 88 88", "4324 888888"),

                new Client("Тестовый клиент9", new DateTime(2000, 12, 2), "testClient9.ru", "+7 918 999 99 99", "4324 999999"),

                new Client("Тестовый клиент10", new DateTime(1980, 8, 17), "testClient10.ru", "+7 918 000 00 00", "4324 000000")

        });

            foreach (var client in clientList)
            {
                clientService.AddClient(client);
            }


            //Act 
            var clients = FakeClientStorage.GetAll();
            int result = clients.Count;

            //Assert
            Assert.Equal(10, result);
            Assert.Equal(1, clients[0].Accounts.Count); // У каждого клиента по 1 дефолтному счетy
            Assert.Equal(1, clients[1].Accounts.Count);
        }

        [Fact]
        public void UpdateClient_Test()
        {
            //Arrange
            IClientStorage FakeClientStorage = new FakeClientStorage();
            ClientService clientService = new ClientService(FakeClientStorage);

            Guid testId = Guid.NewGuid();

            Client testClient = new Client(testId, "Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111");
            clientService.AddClient(testClient);
            
            Client newDataClient = new Client(testId, "Тестовый клиент111", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111");

            //Act
            clientService.UpdateClient(testId, newDataClient);

            //Assert
            Assert.NotEqual(testClient, newDataClient);
            Assert.Equal(testClient.Id, newDataClient.Id);
        }

        [Fact]
        public void DeleteClient_Test()
        {
            //Arrange
            IClientStorage fakeClientStorage = new FakeClientStorage();
            ClientService clientService = new ClientService(fakeClientStorage);

            Guid testId = Guid.NewGuid();

            List<Client> clientList = new List<Client>(); //10
            clientList.AddRange(new Client[]
            {
                new Client(testId, "Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111"),

                new Client("Тестовый клиент2", new DateTime(2001, 11, 2), "testClient2.ru", "+7 918 222 22 22", "4324 222222"),

                new Client("Тестовый клиент3", new DateTime(2002, 12, 31), "testClient3.ru", "+7 918 333 33 33", "4324 333333"),

                new Client("Тестовый клиент4", new DateTime(1999, 10, 24), "testClient4.ru", "+7 918 444 44 44", "4324 444444"),

                new Client("Тестовый клиент5", new DateTime(2003, 8, 8), "testClient5.ru", "+7 918 555 55 55", "4324 555555"),

                new Client("Тестовый клиент6", new DateTime(2002, 7, 6), "testClient6.ru", "+7 918 666 66 66", "4324 666666"),

                new Client("Тестовый клиент7", new DateTime(2001, 8, 21), "testClient7.ru", "+7 918 777 77 77", "4324 777777"),

                new Client("Тестовый клиент8", new DateTime(2002, 10, 4), "testClient8.ru", "+7 918 888 88 88", "4324 888888"),

                new Client("Тестовый клиент9", new DateTime(2000, 12, 2), "testClient9.ru", "+7 918 999 99 99", "4324 999999"),

                new Client("Тестовый клиент10", new DateTime(1980, 8, 17), "testClient10.ru", "+7 918 000 00 00", "4324 000000")

        });

            foreach (var client in clientList)
            {
                clientService.AddClient(client);
            }

            Client client1 = fakeClientStorage.Get(testId);

            //Act
            clientService.DeleteClient(client1.Id);
            bool result = fakeClientStorage.GetAll().Any(x => x.Id == testId);

            //Assert
            Assert.Equal(9, fakeClientStorage.GetAll().Count);
            Assert.False(result);

        }
        [Fact]
        public void AddAccountForActiveClient_Account_Count_2()
        {
            //Arrange 
            IClientStorage fakeClientStorage = new FakeClientStorage();
            ClientService clientService = new ClientService(fakeClientStorage);

            Guid testGuid = Guid.NewGuid();
            List<Client> clientList = new List<Client>(); //10
            clientList.AddRange(new Client[]
            {
                new Client(testGuid, "Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111"),

                new Client("Тестовый клиент2", new DateTime(2001, 11, 2), "testClient2.ru", "+7 918 222 22 22", "4324 222222"),

                new Client("Тестовый клиент3", new DateTime(2002, 12, 31), "testClient3.ru", "+7 918 333 33 33", "4324 333333"),

                new Client("Тестовый клиент4", new DateTime(1999, 10, 24), "testClient4.ru", "+7 918 444 44 44", "4324 444444"),

                new Client("Тестовый клиент5", new DateTime(2003, 8, 8), "testClient5.ru", "+7 918 555 55 55", "4324 555555"),

                new Client("Тестовый клиент6", new DateTime(2002, 7, 6), "testClient6.ru", "+7 918 666 66 66", "4324 666666"),

                new Client("Тестовый клиент7", new DateTime(2001, 8, 21), "testClient7.ru", "+7 918 777 77 77", "4324 777777"),

                new Client("Тестовый клиент8", new DateTime(2002, 10, 4), "testClient8.ru", "+7 918 888 88 88", "4324 888888"),

                new Client("Тестовый клиент9", new DateTime(2000, 12, 2), "testClient9.ru", "+7 918 999 99 99", "4324 999999"),

                new Client("Тестовый клиент10", new DateTime(1980, 8, 17), "testClient10.ru", "+7 918 000 00 00", "4324 000000")

        });

            foreach (var cl in clientList)
            {
                clientService.AddClient(cl);
            }

            Account newAccount = new Account(new Currency("RUB", '#'), 50000, "111 111 111");
            Client Fakeclient = new Client("Тестовый клиент11", new DateTime(2001, 9, 6), "testClient11.ru", "+7 919 111 11 11", "4324 011111");


            //Act 
            Client clientTest = fakeClientStorage.Get(testGuid);
            bool resaltOperation = clientService.AddAccountToClient(clientTest.Id, newAccount);


            // Act & Assert
            Assert.Throws<ClientNotFoundException>(() =>
            {
                clientService.AddAccountToClient(Fakeclient.Id, newAccount);
            });

            //Assert
            Assert.True(resaltOperation);
            Assert.Equal(2, clientTest.Accounts.Count);
        }

        [Fact]
        public void UpdateAccount_Amount_6000()
        {
            //Arrange 
            IClientStorage fakeClientStorage = new FakeClientStorage();
            ClientService clientService = new ClientService(fakeClientStorage);

            Guid testGuid = Guid.NewGuid();
            List<Client> clientList = new List<Client>(); //10
            clientList.AddRange(new Client[]
            {
                new Client( testGuid, "Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111",
                 new Account(new Currency("RUB", '#'), 50000, "111 111 111")),

                new Client("Тестовый клиент2", new DateTime(2001, 11, 2), "testClient2.ru", "+7 918 222 22 22", "4324 222222"),

                new Client("Тестовый клиент3", new DateTime(2002, 12, 31), "testClient3.ru", "+7 918 333 33 33", "4324 333333"),

                new Client("Тестовый клиент4", new DateTime(1999, 10, 24), "testClient4.ru", "+7 918 444 44 44", "4324 444444"),

                new Client("Тестовый клиент5", new DateTime(2003, 8, 8), "testClient5.ru", "+7 918 555 55 55", "4324 555555"),

                new Client("Тестовый клиент6", new DateTime(2002, 7, 6), "testClient6.ru", "+7 918 666 66 66", "4324 666666"),

                new Client("Тестовый клиент7", new DateTime(2001, 8, 21), "testClient7.ru", "+7 918 777 77 77", "4324 777777"),

                new Client("Тестовый клиент8", new DateTime(2002, 10, 4), "testClient8.ru", "+7 918 888 88 88", "4324 888888"),

                new Client("Тестовый клиент9", new DateTime(2000, 12, 2), "testClient9.ru", "+7 918 999 99 99", "4324 999999"),

                new Client("Тестовый клиент10", new DateTime(1980, 8, 17), "testClient10.ru", "+7 918 000 00 00", "4324 000000")
        });

            foreach (var cl in clientList)
            {
                clientService.AddClient(cl);
            }


            //Act 
            Account UpdateAcc = new Account(new Currency("EUR", '€'), 6000);
            bool result = clientService.UpdateAccount(testGuid, "111 111 111", UpdateAcc);

            var client = fakeClientStorage.Get(testGuid);
            client.Accounts.Sort((a, b) => a.Amount.CompareTo(b.Amount));


            //Assert
            Assert.True(result);
            Assert.Equal(2, client.Accounts.Count);
            Assert.Equal(6000, client.Accounts[1].Amount);
            Assert.Equal("EUR", client.Accounts[1].Currency.Code);
        }

        [Fact]
        public void DeleteAccount_Test()
        {
            //Arrange
            IClientStorage FakeClientStorage = new FakeClientStorage();
            ClientService clientService = new ClientService(FakeClientStorage);

            Guid testId = Guid.NewGuid();

            Client testClient = new Client(testId, "Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111");
            clientService.AddClient(testClient);
            clientService.AddAccountToClient(testClient.Id , new Account(new Currency("USD", '$'), 4000, "123"));

            //Act
            bool result = clientService.DeleteAccount(testClient.Id, "123");

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GetFilterClients()
        {
            //Arrange 
            IClientStorage fakeClientStorage = new FakeClientStorage();
            ClientService clientService = new ClientService(fakeClientStorage);

            List<Client> clientList = new List<Client>(); //10
            clientList.AddRange(new Client[]
            {
                new Client("Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111",
                 new Account(new Currency("RUB", '#'), 50000, "111 111 111")),

                new Client("Тестовый клиент2", new DateTime(2001, 11, 2), "testClient2.ru", "+7 918 222 22 22", "4324 222222"),

                new Client("Тестовый клиент3", new DateTime(2002, 12, 31), "testClient3.ru", "+7 918 333 33 33", "4324 333333"),

                new Client("Тестовый клиент4", new DateTime(1999, 10, 24), "testClient4.ru", "+7 918 444 44 44", "4324 444444"),

                new Client("Тестовый клиент5", new DateTime(2003, 8, 8), "testClient5.ru", "+7 918 555 55 55", "4324 555555"),

                new Client("Тестовый клиент6", new DateTime(2002, 7, 6), "testClient6.ru", "+7 918 666 66 66", "4324 666666"),

                new Client("Тестовый клиент7", new DateTime(2001, 8, 21), "testClient7.ru", "+7 918 777 77 77", "4324 777777"),

                new Client("Тестовый клиент8", new DateTime(2002, 10, 4), "testClient8.ru", "+7 918 888 88 88", "4324 888888"),

                new Client("Тестовый клиент9", new DateTime(2000, 12, 2), "testClient9.ru", "+7 918 999 99 99", "4324 999999"), 

                new Client("Тестовый клиент10", new DateTime(1980, 8, 17), "testClient10.ru", "+7 918 000 00 00", "4324 000000")
        });

            foreach (var cl in clientList)
            {
                clientService.AddClient(cl);
            }


            //Act

            ClientFilterDTO filter = new ClientFilterDTO
            {
                FullName = "Тестовый",
                BirthDateFrom = new DateTime(2000, 1, 1),
                BirthDateTo = new DateTime(2002, 1, 1)
            };

            var filterClients = clientService.GetFilterClients(filter, 1).Items;


            //Assert
            Assert.Equal(4, filterClients.Count);
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент1"));
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент2"));
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент7"));
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент9"));
        }
    }
}
