using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class FakeClientStorage  : IClientStorage
    {
        private Dictionary<Guid, Client> _allBankClients = new Dictionary<Guid, Client>();
        public IReadOnlyDictionary<Guid, Client> AllBankClients => _allBankClients;

        public List<Client> Get(Func<Client, bool>? predicate = null)
        {
            var clients = _allBankClients.Values.ToList();

            if (predicate != null)
                clients = clients.Where(predicate).ToList();

            return clients;
        }
        public bool Add(Client client)
        {
            if (_allBankClients.ContainsKey(client.Id))
                return false;

            _allBankClients.Add(client.Id, client);
            return true;
        }

        public bool Update(Client client)
        {
            if (client == null)
                return false;
            if (!_allBankClients.ContainsKey(client.Id))
                return false;

            _allBankClients[client.Id] = client;
            return true;
        }

        public bool Delete(Client client)
        {
            if (client == null)
                return false;

            if (!_allBankClients.ContainsKey(client.Id))
                return false;

            return _allBankClients.Remove(client.Id);
        }

        public bool AddAccount(Guid clientId, Account account)
        {
            if (!_allBankClients.ContainsKey(clientId))
                return false;

            if (account == null)
                return false;

            _allBankClients[clientId].Accounts.Add(account);
            return true;
        }

        public bool UpdateAccount(Guid clientId, string accountNumber, Account newAccount)
        {
            if (!_allBankClients.TryGetValue(clientId, out Client client))
                return false;

            Account account1 = client.Accounts.FirstOrDefault(u => u.AccountNumber == accountNumber);

            if (account1 == null)
                return false;

            account1.EditAccount(newAccount.Currency, newAccount.Amount);
            return true;
        }

        public bool DeleteAccount(Guid clientId, string accountNumber)
        {
            if (!_allBankClients.TryGetValue(clientId, out Client client))
                return false;

            if (string.IsNullOrWhiteSpace(accountNumber))
                return false;

            var accountToRemove = _allBankClients[clientId].Accounts.FirstOrDefault(u => u.AccountNumber == accountNumber);

            if (accountToRemove == null)
                throw new KeyNotFoundException("Ошибка: некорректный номер счета");

            client.Accounts.Remove(accountToRemove);

            return true;
        }
    }
}
