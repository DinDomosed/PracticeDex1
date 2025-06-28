using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class ClientService
    {
        private readonly ClientStorage _clientStorage;
        public ClientService(ClientStorage clientStorage)
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
                throw new PassportNumberNullOrWhiteSpaceException("Неккоректный ввод серии и номера паспорта");
            }
            

            client.Accounts.Add(new Account(new Currency("USD", '$'), 0));
            _clientStorage.AddClientToStorage(client);
            return true;
        }

        public bool AddAccountForActiveClient(Account account, Client client)
        {
            if (account == null)
                throw new ArgumentNullException("Ошибка: добавляемый лицевой счет не может быть null");

            if (!_clientStorage.AllBankClients.ContainsKey(client.Id))
                throw new ArgumentException("Ошибка: такого клиента нет в базе");

            _clientStorage.AllBankClients.TryGetValue(client.Id, out Client client1);

            if (client1 != null)
                client1.Accounts.Add(account);
            return true;
        }
        public bool EditingAccount(string fullName, string passportNumber, string accountNumber, string newCurrencyCode, char newCurrencySymbol, decimal newAmount)
        {
            if (string.IsNullOrWhiteSpace(passportNumber))
                throw new PassportNumberNullOrWhiteSpaceException("Паспорт не должен быть пустым");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentNullException("Имя не может быть пустым");
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentNullException("Номер лицевого счета не может быть пустым");
            if (string.IsNullOrWhiteSpace(newCurrencyCode))
                throw new ArgumentNullException("Код вылюты не может быть пустым");
            if (newCurrencySymbol.Equals(default(char)))
                throw new ArgumentNullException("Символ валюты не может быть пустым");
            if (newAmount < 0)
                throw new ArgumentOutOfRangeException("Баланс не может быть отрицательным");

            var clientPair = _clientStorage.AllBankClients.FirstOrDefault(u => u.Value.FullName == fullName && u.Value.PassportNumber == passportNumber);
            if (clientPair.Equals(default(KeyValuePair<Guid, Client>)))
                throw new ClientNotFoundException("Клиент с такими данными не найден");

            var account = clientPair.Value.Accounts.FirstOrDefault(u => u.AccountNumber == accountNumber)
                 ?? throw new AccountNotFoundException("Счет с таким номером не найден");

            account.EditAccount(new Currency(newCurrencyCode, newCurrencySymbol), newAmount);
            return true;
        }

        public List<Client> GetFilterClients(string? fullName, string? phoneNumber, string? passportNumber, DateTime? fromThisDate, DateTime beforeThisDate)
        {  
            var clients = _clientStorage.AllBankClients.Values.AsQueryable();

            if (!string.IsNullOrWhiteSpace(fullName))
                clients = clients.Where(u => u.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                clients = clients.Where(u => u.PhoneNumber == phoneNumber);

            if (!string.IsNullOrWhiteSpace(passportNumber))
                clients = clients.Where(u => u.PassportNumber == passportNumber);

            if (fromThisDate != null)
                clients = clients.Where(u => u.Birthday >= fromThisDate);

            if (beforeThisDate != null)
            {
                if (fromThisDate != null && beforeThisDate > fromThisDate)
                    clients = clients.Where(u => u.Birthday <= beforeThisDate && u.Birthday >= fromThisDate);
                else
                    clients = clients.Where(u => u.Birthday <= beforeThisDate);
            }

            return clients.ToList();
        }
    }
}
