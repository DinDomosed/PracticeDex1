using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
        public bool AddClient(Client client)
        {
            int minAgeLimit = 18;

            if (client.Age < minAgeLimit)
            {
                throw new InvalidClientAgeException("Ошибка: Минимальный возраст клиента 18 лет");
            }
            if (string.IsNullOrWhiteSpace(client.PassportNumber))
            {
                throw new PassportNumberNullOrWhiteSpaceException("Некорректный ввод серии и номера паспорта");
            }


            client.Accounts.Add(new Account(new Currency("USD", '$'), 0)); 
            return _clientStorage.Add(client);
        }
        public bool UpdateClient(Client client)
        {
            if(client == null)
                throw new ArgumentNullException(nameof(client), "Ошибка: Клиент не может быть null");

            if (!_clientStorage.Get().Any(u => u.Id == client.Id))
                throw new ClientNotFoundException("Ошибка: Клиент не найден");

            if (client.Age < 18)
                throw new InvalidClientAgeException("Ошибка: Минимальный возраст клиента 18 лет");

            if (string.IsNullOrWhiteSpace(client.PassportNumber))
                throw new PassportNumberNullOrWhiteSpaceException("Ошибка: Неккоректный ввод серии и номера паспорта");

            if (string.IsNullOrWhiteSpace(client.PhoneNumber))
                throw new ArgumentNullException("Ошибка: Некорректный номер телефона");

            return _clientStorage.Update(client);
        }
        public bool DeleteClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException("Ошибка: Клиент не может быть null");
            if (!_clientStorage.Get().Any(u => u.Id == client.Id))
                throw new ClientNotFoundException("Ошибка: Клиент не найден");

            return _clientStorage.Delete(client);
        }

        public bool AddAccountForActiveClient(Account newAccount, Client client)
        {
            if (newAccount == null)
                throw new ArgumentNullException("Ошибка: добавляемый лицевой счет не может быть null");
            var clientStorage = _clientStorage.Get(u => u.Id == client.Id);

            if (!clientStorage.Any(u => u.Id == client.Id))
                throw new ClientNotFoundException("Ошибка: такого клиента нет в базе");

            
            return _clientStorage.AddAccount(client.Id, newAccount);
        }
        public bool UpdateAccount(Guid IdClient , string accountNumber, Account newAccount)
        {
            var client =_clientStorage.Get(u => u.Id == IdClient).FirstOrDefault()
                ?? throw new ClientNotFoundException("Ошибка: клиент не найден");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentNullException("Ошибка: некорректный номер счета");

            if (!client.Accounts.Any(u => u.AccountNumber == accountNumber))
                throw new AccountNotFoundException("Ошибка: счет не найден");

            if(newAccount == null)
                throw new ArgumentNullException("Ошибка: cчет не может быть null");

            return _clientStorage.UpdateAccount(IdClient, accountNumber, newAccount);
        }
        public bool DeleteAccount(Guid IdClient , string accountNumber)
        {
            if (!_clientStorage.Get().Any(u => u.Id == IdClient))
                throw new ClientNotFoundException("Ошибка: Клиент не найден");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentNullException("Ошибка: номер счета не может быть null");

            return _clientStorage.DeleteAccount(IdClient, accountNumber);

        }

        public List<Client> GetFilterClients(string? fullName = null, string? phoneNumber = null, string? passportNumber = null, DateTime? fromThisDate = null, DateTime? beforeThisDate = null)
        {
            var clients = _clientStorage.Get();

            if (!string.IsNullOrWhiteSpace(fullName))
                clients = clients.Where(u => u.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                clients = clients.Where(u => u.PhoneNumber == phoneNumber).ToList();

            if (!string.IsNullOrWhiteSpace(passportNumber))
                clients = clients.Where(u => u.PassportNumber == passportNumber).ToList();

            if (fromThisDate.HasValue)
                clients = clients.Where(u => u.Birthday >= fromThisDate).ToList();

            if (beforeThisDate.HasValue)
                clients = clients.Where(u => u.Birthday <= beforeThisDate).ToList();


            return clients.ToList();
        }
    }
}
