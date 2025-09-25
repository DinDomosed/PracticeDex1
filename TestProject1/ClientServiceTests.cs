using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.DTOs.DTOsForRequestsToControllers;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task CashingOutMoney_memoryTest()
        {
            //Arrange 
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            List<Client> clientList = new List<Client>();
            clientList.AddRange(new Client[]
            {
                new Client(new Guid(" 9f8b1c6e-2d4f-4a8a-9b2e-3f1d7e6a5b8c"),"Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111"),

                new Client(new Guid("4e7a2d1b-5c3f-4b9e-8d7a-2f6c1e3b9a0d"),"Тестовый клиент2", new DateTime(2001, 11, 2), "testClient2.ru", "+7 918 222 22 22", "4324 222222"),

                new Client(new Guid("a6d3f1b9-7e2c-4a5b-9f8d-1c3e6b7a2d4f"),"Тестовый клиент3", new DateTime(2002, 12, 31), "testClient3.ru", "+7 918 333 33 33", "4324 333333"),
            });

            foreach (var client in clientList)
            {
                await clientStorage.AddAsync(client);
            }

            var clientsfromStorage = await clientStorage.GetAllAsync();

            foreach (var client in clientsfromStorage)
            {
                foreach(var account in client.Accounts)
                {
                    account.PutMoney(1000);
                }
            }


            //Act

            var task1 = Task.Run(async () =>
            {
                var theWithdrawingClient = await clientService.GetAsync(new Guid(" 9f8b1c6e-2d4f-4a8a-9b2e-3f1d7e6a5b8c"));
                var foundAccount = theWithdrawingClient.Accounts.First();

                return await clientService.CashingOutMoney(theWithdrawingClient.Id, foundAccount.Id, 300);
            });

            var task2 = Task.Run(async () =>
            {
                var theWithdrawingClient = await clientService.GetAsync(new Guid("4e7a2d1b-5c3f-4b9e-8d7a-2f6c1e3b9a0d"));
                var foundAccount = theWithdrawingClient.Accounts.First();

                return await clientService.CashingOutMoney(theWithdrawingClient.Id, foundAccount.Id, 100);
            });

            var task3 = Task.Run(async () =>
            {
                var theWithdrawingClient = await clientService.GetAsync(new Guid("a6d3f1b9-7e2c-4a5b-9f8d-1c3e6b7a2d4f"));
                var foundAccount = theWithdrawingClient.Accounts.First();

                return await clientService.CashingOutMoney(theWithdrawingClient.Id, foundAccount.Id,1100);
            });

            var result = await Task.WhenAll(task1, task2, task3);

            //Assert
            Assert.Equal(700, clientsfromStorage.FirstOrDefault(c => c.Id == new Guid(" 9f8b1c6e-2d4f-4a8a-9b2e-3f1d7e6a5b8c")).Accounts.First().Amount);
            Assert.Equal(900, clientsfromStorage.FirstOrDefault(c => c.Id == new Guid("4e7a2d1b-5c3f-4b9e-8d7a-2f6c1e3b9a0d")).Accounts.First().Amount);
            Assert.Equal(1000, clientsfromStorage.FirstOrDefault(c => c.Id == new Guid("a6d3f1b9-7e2c-4a5b-9f8d-1c3e6b7a2d4f")).Accounts.First().Amount);
            Assert.True(result[0]);
            Assert.True(result[1]);
            Assert.False(result[2]);
        }


        [Fact]
        public async Task AddClient_Count_10()
        {
            //Arrange
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

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
                await clientService.AddClientAsync(client);
            }


            //Act 
            var clients = await clientStorage.GetAllAsync();
            int result = clients.Count;

            //Assert
            Assert.Equal(10, result);
            Assert.Equal(1, clients[0].Accounts.Count); // У каждого клиента по 1 дефолтному счетy
            Assert.Equal(1, clients[1].Accounts.Count);
        }

        [Fact]
        public async Task UpdateClient_Test()
        {
            //Arrange
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            Guid testId = Guid.NewGuid();

            Client testClient = new Client(testId, "Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111");
            await clientService.AddClientAsync(testClient);

            Client newDataClient = new Client(testId, "Тестовый клиент111", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111");

            //Act
            clientService.UpdateClientAsync(testId, newDataClient);

            //Assert
            Assert.NotEqual(testClient, newDataClient);
            Assert.Equal(testClient.Id, newDataClient.Id);
        }

        [Fact]
        public async Task DeleteClient_Test()
        {
            //Arrange
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

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
                await clientService.AddClientAsync(client);
            }

            Client client1 = await clientStorage.GetAsync(testId);

            //Act
            clientService.DeleteClientAsync(client1.Id);
            var allClients = await clientStorage.GetAllAsync();
            bool result = allClients.Any(x => x.Id == testId);

            //Assert
            Assert.Equal(9, allClients.Count);
            Assert.False(result);

        }
        [Fact]
        public async Task AddAccountForActiveClient_Account_Count_2()
        {
            //Arrange 
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

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
                await clientService.AddClientAsync(cl);
            }

            Account newAccount = new Account(testGuid, new Currency("RUB", '#'), 50000, "111 111 111");
            Client Fakeclient = new Client("Тестовый клиент11", new DateTime(2001, 9, 6), "testClient11.ru", "+7 919 111 11 11", "4324 011111");


            //Act 
            Client clientTest = await clientStorage.GetAsync(testGuid);
            bool resaltOperation = await clientService.AddAccountToClientAsync(clientTest.Id, newAccount);


            // Act & Assert
            await Assert.ThrowsAsync<ClientNotFoundException>(async () =>
            {
                await clientService.AddAccountToClientAsync(Fakeclient.Id, newAccount);
            });

            //Assert
            Assert.True(resaltOperation);
            Assert.Equal(2, clientTest.Accounts.Count);
        }

        [Fact]
        public async Task UpdateAccount_Amount_6000()
        {
            //Arrange 
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            Guid testGuid = Guid.NewGuid();
            List<Client> clientList = new List<Client>(); //10
            clientList.AddRange(new Client[]
            {
                new Client( testGuid, "Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111",
                 new Account(testGuid, new Currency("RUB", '#'), 50000, "111 111 111")),

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
                await clientService.AddClientAsync(cl);
            }


            //Act 
            Account UpdateAcc = new Account(testGuid, new Currency("EUR", '€'), 6000);
            bool result = await clientService.UpdateAccountAsync(testGuid, "111 111 111", UpdateAcc);

            var client = await clientStorage.GetAsync(testGuid);
            client.Accounts.Sort((a, b) => a.Amount.CompareTo(b.Amount));


            //Assert
            Assert.True(result);
            Assert.Equal(2, client.Accounts.Count);
            Assert.Equal(6000, client.Accounts[1].Amount);
            Assert.Equal("EUR", client.Accounts[1].Currency.Code);
        }

        [Fact]
        public async Task DeleteAccount_Test()
        {
            //Arrange
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            Guid testId = Guid.NewGuid();

            Client testClient = new Client(testId, "Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111");
            await clientService.AddClientAsync(testClient);
            clientService.AddAccountToClientAsync(testClient.Id, new Account(testClient.Id, new Currency("USD", '$'), 4000, "123"));

            //Act
            bool result = await clientService.DeleteAccountAsync(testClient.Id, "123");

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetFilterClients()
        {
            //Arrange 
            IClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

            List<Client> clientList = new List<Client>(); //10
            clientList.AddRange(new Client[]
            {
                new Client("Тестовый клиент1", new DateTime(2000, 9, 6), "testClient1.ru", "+7 918 111 11 11", "4324 111111"),
                 /*new Account(new Currency("RUB", '#'), 50000, "111 111 111"))*/

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
                await clientService.AddClientAsync(cl);
            }


            //Act

            ClientFilterDTO filter = new ClientFilterDTO
            {
                FullName = "Тестовый",
                BirthDateFrom = new DateTime(2000, 1, 1),
                BirthDateTo = new DateTime(2002, 1, 1)
            };

            var filterResult = await clientService.GetFilterClientsAsync(filter, 1);
            var filterClients = filterResult.Items;

            //Assert
            Assert.Equal(4, filterClients.Count);
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент1"));
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент2"));
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент7"));
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент9"));
        }
    }
}
