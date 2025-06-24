using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages
{
    public class ClientStorage
    {
        private Dictionary<Guid, Client> _allBankClients = new Dictionary<Guid, Client>();
        public IReadOnlyDictionary<Guid, Client> AllBankClients => _allBankClients;

        public bool AddClientToStorage(Client client)
        {
            if(_allBankClients.ContainsKey(client.Id))
                return false;

            _allBankClients.Add(client.Id, client);
            return true;
        }
    }
}
