using BankSystem.App.Common;
using BankSystem.App.DTOs.DTOsForRequestsToControllers;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace BankSystem.Data.Storages
{
    public class ClientStorage : IClientStorage
    {
        private Dictionary<Guid, Client> _allBankClients = new Dictionary<Guid, Client>();
        public IReadOnlyDictionary<Guid, Client> AllBankClients => _allBankClients;

        public async Task<Client?> GetAsync(Guid Id)
        {

            if (!_allBankClients.ContainsKey(Id))
                return null;

            return _allBankClients[Id];
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return _allBankClients.Values.ToList();
        }
        public async Task<bool> AddAsync(Client client)
        {
            if (_allBankClients.ContainsKey(client.Id))
                return false;

            client.Accounts.Add(new Account(client.Id, new Currency("USD", '$'), 0));
            _allBankClients.Add(client.Id, client);
            return true;
        }

        public async Task<bool> UpdateAsync(Guid Id, Client upClient)
        {
            if (Id == Guid.Empty)
                return false;
            if (upClient == null)
                return false;

            if (!_allBankClients.ContainsKey(Id))
                return false;

            _allBankClients[Id] = upClient;
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            if (!_allBankClients.ContainsKey(id))
                return false;

            return _allBankClients.Remove(id);
        }

        public async Task<bool> AddAccountAsync(Guid clientId, Account account)
        {
            if (account == null)
                return false;

            if (!_allBankClients.ContainsKey(clientId))
                return false;

            _allBankClients[clientId].Accounts.Add(account);
            return true;
        }

        public async Task<bool> UpdateAccountAsync(Guid clientId, string accountNumber, Account newAccount)
        {
            if (!_allBankClients.TryGetValue(clientId, out Client client))
                return false;

            Account foundAccount = client.Accounts.FirstOrDefault(u => u.AccountNumber == accountNumber);

            if (foundAccount == null)
                return false;

            foundAccount.EditAccount(newAccount.Currency, newAccount.Amount);
            return true;
        }

        public async Task<bool> DeleteAccountAsync(Guid clientId, string accountNumber)
        {
            if (!_allBankClients.TryGetValue(clientId, out Client client))
                return false;

            if (string.IsNullOrWhiteSpace(accountNumber))
                return false;

            var accountToRemove = _allBankClients[clientId].Accounts.FirstOrDefault(u => u.AccountNumber == accountNumber);

            if (accountToRemove == null)
                return false;

            client.Accounts.Remove(accountToRemove);

            return true;
        }

        public async Task<PagedResult<Client>> GetFilterClientsAsync(ClientFilterDTO filter, int page, int pageSize)
        {
            IEnumerable<Client> clients = _allBankClients.Values;

            if (!string.IsNullOrWhiteSpace(filter.FullName))
                clients = clients.Where(c => c.FullName.Contains(filter.FullName));

            if (filter.BirthDateFrom.HasValue)
                clients = clients.Where(c => c.Birthday >= filter.BirthDateFrom);

            if (filter.BirthDateTo.HasValue)
                clients = clients.Where(c => c.Birthday <= filter.BirthDateTo);

            if (filter.Bonus.HasValue)
                clients = clients.Where(c => c.Bonus == filter.Bonus);

            if (filter.CountAccounts.HasValue)
                clients = clients.Where(c => c.Accounts.Count == filter.CountAccounts);

            if (!string.IsNullOrWhiteSpace(filter.Email))
                clients = clients.Where(c => c.Email.ToLower().Contains(filter.Email.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
                clients = clients.Where(c => c.PhoneNumber == filter.PhoneNumber);

            if (!string.IsNullOrWhiteSpace(filter.PassportNumber))
                clients = clients.Where(c => c.PassportNumber.Trim() == filter.PassportNumber.Trim());

            if (filter.RegisterDateFrom.HasValue)
                clients = clients.Where(c => c.RegistrationDate >= filter.RegisterDateFrom);

            if (filter.RegisterDateTo.HasValue)
                clients = clients.Where(c => c.RegistrationDate <= filter.RegisterDateTo);

            int totalCount = clients.Count();

            var items = clients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Client>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
        public async Task<bool> ExistsAsync(Guid id, string passportNumber)
        {
            return _allBankClients.Any(c => c.Value.Id == id || c.Value.PassportNumber == passportNumber);
        }
    }
}
