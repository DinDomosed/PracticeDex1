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
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        public ClientService(IClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }
        
        public async Task<bool> CashingOutMoney(Guid clientId, Guid accountId, decimal amount)
        {
            if (clientId == Guid.Empty || accountId == Guid.Empty)
                return false;


            await _semaphoreSlim.WaitAsync();
            try
            {
                var foundClient = await _clientStorage.GetAsync(clientId);

                if (foundClient == null)
                    return false;

                var foundAccount = foundClient.Accounts.FirstOrDefault(c => c.Id == accountId);

                if (foundAccount == null)
                    return false;

                var currentAmount = foundAccount.Amount;
                foundAccount.GetMoney(amount);

                if (foundAccount.Amount == currentAmount)
                {
                    return false;
                }

                return await _clientStorage.UpdateAccountAsync(foundClient.Id, foundAccount.AccountNumber, foundAccount);
            }
            finally
            {
                _semaphoreSlim.Release(); 
            }
        }

        public async Task<Client?> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID ");

            return await _clientStorage.GetAsync(id);
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _clientStorage.GetAllAsync();
        }

        public async Task<bool> AddClientAsync(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "Ошибка: Клиент не может быть null");

            if (await _clientStorage.ExistsAsync(client.Id, client.PassportNumber))
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

            return await _clientStorage.AddAsync(client);
        }
        public async Task<bool> UpdateClientAsync(Guid idClient, Client client)
        {
            if (idClient == Guid.Empty)
                throw new ArgumentNullException(nameof(idClient), "Ошибка: Некорректный ID");

            if (client == null)
                throw new ArgumentNullException(nameof(client), "Ошибка: Клиент не может быть null");

            var foundClient = await _clientStorage.GetAsync(idClient);

            if (foundClient == null)
                throw new ClientNotFoundException("Ошибка: Клиент не найден");

            if (client.Age < 18)
                throw new InvalidClientAgeException("Ошибка: Минимальный возраст клиента 18 лет");

            if (string.IsNullOrWhiteSpace(client.PassportNumber))
                throw new PassportNumberNullOrWhiteSpaceException("Ошибка: Неккоректный ввод серии и номера паспорта");

            if (string.IsNullOrWhiteSpace(client.PhoneNumber))
                throw new ArgumentException("Ошибка: Некорректный номер телефона");

            return await _clientStorage.UpdateAsync(idClient, client);
        }
        public async Task<bool> DeleteClientAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("Ошибка: Некорректный ID");

            var foundClient = await _clientStorage.GetAsync(id);

            if (foundClient == null)
                throw new ClientNotFoundException("Ошибка: Клиент не найден");

            return await _clientStorage.DeleteAsync(foundClient.Id);
        }

        public async Task<bool> AddAccountToClientAsync(Guid clientID, Account newAccount)
        {
            if (newAccount == null)
                throw new ArgumentNullException("Ошибка: добавляемый лицевой счет не может быть null");

            var clientStorage = await _clientStorage.GetAsync(clientID);

            if (clientStorage == null)
                throw new ClientNotFoundException("Ошибка: такого клиента нет в базе");


            return await _clientStorage.AddAccountAsync(clientStorage.Id, newAccount);
        }
        public async Task<bool> UpdateAccountAsync(Guid IdClient, string accountNumber, Account newAccount)
        {
            var client = await _clientStorage.GetAsync(IdClient)
                ?? throw new ClientNotFoundException("Ошибка: клиент не найден");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Ошибка: некорректный номер счета");

            if (!client.Accounts.Any(u => u.AccountNumber == accountNumber))
                throw new AccountNotFoundException("Ошибка: счет не найден");

            if (newAccount == null)
                throw new ArgumentNullException("Ошибка: cчет не может быть null");

            return await _clientStorage.UpdateAccountAsync(IdClient, accountNumber, newAccount);
        }
        public async Task<bool> DeleteAccountAsync(Guid IdClient, string accountNumber)
        {
            if (IdClient == Guid.Empty)
                throw new ArgumentNullException(nameof(IdClient), "Ошибка: Некорректный ID");

            var foundClient = await _clientStorage.GetAsync(IdClient);

            if (foundClient == null)
                throw new ClientNotFoundException("Ошибка: Клиент не найден");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentNullException("Ошибка: Некорректный номер счета");

            var foundAccount = foundClient.Accounts.FirstOrDefault(c => c.AccountNumber == accountNumber);

            if (foundAccount == null)
                throw new AccountNotFoundException("Ошибка: У клиента нет аккаунта с таким номером");

            return await _clientStorage.DeleteAccountAsync(IdClient, accountNumber);

        }

        public async Task<PagedResult<Client>> GetFilterClientsAsync(ClientFilterDTO filter, int page, int pageSize = 10)
        {
            if (page.Equals(default(int)))
                throw new ArgumentException(nameof(page), "Ошибка страницы");

            if (pageSize.Equals(default(int)))
                throw new ArgumentException(nameof(pageSize), "Ошибка: некорректный размер страницы");

            return await _clientStorage.GetFilterClientsAsync(filter, page, pageSize);
        }
    }
}
