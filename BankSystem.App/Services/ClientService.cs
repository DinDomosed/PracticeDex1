using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Common;
using BankSystem.App.DTOs;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;


namespace BankSystem.App.Services
{
    public class ClientService
    {
        private readonly IClientStorage _clientStorage;
        public ClientService(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }
        public Client? Get(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID ");

            return _clientStorage.Get(id);
        }

        public List<Client> GetAll()
        {
            return _clientStorage.GetAll();
        }

        public bool AddClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "Ошибка: Клиент не может быть null");

            if (_clientStorage.Exists(client.Id, client.PassportNumber))
                throw new ArgumentException(nameof(client), "Клиент уже зарегестрирован");
                
            int minAgeLimit = 18;

            if (client.Age < minAgeLimit)
            {
                throw new InvalidClientAgeException("Ошибка: Минимальный возраст клиента 18 лет");
            }
            if (string.IsNullOrWhiteSpace(client.PassportNumber))
            {
                throw new PassportNumberNullOrWhiteSpaceException("Некорректный ввод серии и номера паспорта");
            }

            return _clientStorage.Add(client);
        }
        public bool UpdateClient(Guid idClient, Client client)
        {
            if (idClient == Guid.Empty)
                throw new ArgumentNullException(nameof(idClient), "Ошибка: Некорректный ID");

            if (client == null)
                throw new ArgumentNullException(nameof(client), "Ошибка: Клиент не может быть null");

            var foundClient = _clientStorage.Get(idClient);

            if (foundClient == null)
                throw new ClientNotFoundException("Ошибка: Клиент не найден");

            if (client.Age < 18)
                throw new InvalidClientAgeException("Ошибка: Минимальный возраст клиента 18 лет");

            if (string.IsNullOrWhiteSpace(client.PassportNumber))
                throw new PassportNumberNullOrWhiteSpaceException("Ошибка: Неккоректный ввод серии и номера паспорта");

            if (string.IsNullOrWhiteSpace(client.PhoneNumber))
                throw new ArgumentException("Ошибка: Некорректный номер телефона");

            return _clientStorage.Update(idClient, client);
        }
        public bool DeleteClient(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("Ошибка: Некорректный ID");

            var foundClient = _clientStorage.Get(id);

            if (foundClient == null)
                throw new ClientNotFoundException("Ошибка: Клиент не найден");

            return _clientStorage.Delete(foundClient.Id);
        }

        public bool AddAccountToClient(Guid clientID, Account newAccount)
        {
            if (newAccount == null)
                throw new ArgumentNullException("Ошибка: добавляемый лицевой счет не может быть null");

            var clientStorage = _clientStorage.Get(clientID);

            if (clientStorage == null)
                throw new ClientNotFoundException("Ошибка: такого клиента нет в базе");


            return _clientStorage.AddAccount(clientStorage.Id, newAccount);
        }
        public bool UpdateAccount(Guid IdClient, string accountNumber, Account newAccount)
        {
            var client = _clientStorage.Get(IdClient)
                ?? throw new ClientNotFoundException("Ошибка: клиент не найден");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Ошибка: некорректный номер счета");

            if (!client.Accounts.Any(u => u.AccountNumber == accountNumber))
                throw new AccountNotFoundException("Ошибка: счет не найден");

            if (newAccount == null)
                throw new ArgumentNullException("Ошибка: cчет не может быть null");

            return _clientStorage.UpdateAccount(IdClient, accountNumber, newAccount);
        }
        public bool DeleteAccount(Guid IdClient, string accountNumber)
        {
            if (IdClient == Guid.Empty)
                throw new ArgumentNullException(nameof(IdClient), "Ошибка: Некорректный ID");

            var foundClient = _clientStorage.Get(IdClient);

            if (foundClient == null)
                throw new ClientNotFoundException("Ошибка: Клиент не найден");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentNullException("Ошибка: Некорректный номер счета");

            var foundAccount = foundClient.Accounts.FirstOrDefault(c => c.AccountNumber == accountNumber);

            if (foundAccount == null)
                throw new AccountNotFoundException("Ошибка: У клиента нет аккаунта с таким номером");

            return _clientStorage.DeleteAccount(IdClient, accountNumber);

        }

        public PagedResult<Client> GetFilterClients(ClientFilterDTO filter, int page, int pageSize = 10)
        {
            if (page.Equals(default(int)))
                throw new ArgumentException(nameof(page), "Ошибка страницы");

            if (pageSize.Equals(default(int)))
                throw new ArgumentException(nameof(pageSize), "Ошибка: некорректный размер страницы");

            return _clientStorage.GetFilterClients(filter, page, pageSize);
        }
    }
}
