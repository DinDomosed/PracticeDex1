using BankSystem.App.Common;
using BankSystem.App.DTOs;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IClientStorage : IStorage<Client>
    {
        public Task<bool> AddAccountAsync(Guid clientID, Account account);
        public Task<bool> UpdateAccountAsync(Guid clientID, string accountNumber, Account account);
        public Task<bool> DeleteAccountAsync(Guid clientID, string accountNumber);
        public Task<PagedResult<Client>> GetFilterClientsAsync(ClientFilterDTO filter, int page, int pageSize);
    }
}
