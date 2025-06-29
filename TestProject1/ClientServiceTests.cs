using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class ClientServiceTests
    {
        [Fact]
        public void AddClient_Count_10()
        {
            //Arrange
            ClientStorage clientStorage = new ClientStorage();
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
                clientService.AddClient(client);
            }


            //Act 
            int result = clientStorage.AllBankClients.Count;
            List<Client> clients = clientStorage.AllBankClients.Values.ToList();


            //Assert
            Assert.Equal(10, result);
            Assert.Equal(1, clientList[0].Accounts.Count); // У каждого клиента по 1 дефолтному счету
        }


        [Fact]
        public void AddAccountForActiveClient_Account_Count_2()
        {
            //Arrange 
            ClientStorage clientStorage = new ClientStorage();
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
                clientService.AddClient(cl);
            }

            Account newAccount = new Account(new Currency("RUB", '#'), 50000, "111 111 111");
            Client client = new Client("Тестовый клиент11", new DateTime(2001, 9, 6), "testClient11.ru", "+7 919 111 11 11", "4324 011111");


            //Act 
            Client clientTest = clientStorage.AllBankClients[testGuid];
            bool resaltOperation = clientService.AddAccountForActiveClient(newAccount, clientTest);


            // Act & Assert
            Assert.Throws<ClientNotFoundException>(() =>
            {
                clientService.AddAccountForActiveClient(newAccount, client);
            });

            //Assert
            Assert.True(resaltOperation);
            Assert.Equal(2, clientStorage.AllBankClients[testGuid].Accounts.Count);
        }


        [Fact]
        public void EditingAccount_Amount_6000()
        {
            //Arrange 
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

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
            bool result = clientService.EditingAccount(testGuid, "Тестовый клиент1", "4324 111111", "111 111 111", "EUR", '€', 6000);
            clientStorage.AllBankClients[testGuid].Accounts.Sort((a, b) => a.Amount.CompareTo(b.Amount));


            //Assert
            Assert.True(result);
            Assert.Equal(2, clientStorage.AllBankClients[testGuid].Accounts.Count);
            Assert.Equal(6000, clientStorage.AllBankClients[testGuid].Accounts[1].Amount);
        }

        [Fact]
        public void GetFilterClients()
        {
            //Arrange 
            ClientStorage clientStorage = new ClientStorage();
            ClientService clientService = new ClientService(clientStorage);

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
            var filterClients = clientService.GetFilterClients(fullName: "Тестовый", fromThisDate: new DateTime(2000, 1, 1), beforeThisDate: new DateTime(2002, 1, 1));


            //Assert
            Assert.Equal(4, filterClients.Count);
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент1"));
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент2"));
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент7"));
            Assert.True(filterClients.Any(n => n.FullName == "Тестовый клиент9"));
        }
    }
}
