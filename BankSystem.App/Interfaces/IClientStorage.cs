using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IClientStorage : IStorage<Client>
    {
        public bool AddAccount(Guid clientID, Account account);
        public bool UpdateAccount(Guid clientID, string accountNumber, Account account);
        public bool DeleteAccount(Guid clientID, string accountNumber);

    }
}
