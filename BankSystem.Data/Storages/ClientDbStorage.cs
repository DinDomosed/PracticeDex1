using BankSystem.App.Common;
using BankSystem.App.DTOs.DTOsForRequestsToControllers;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class ClientDbStorage : IClientStorage
    {
        private readonly BankSystemDbContext _dbContext;

        public ClientDbStorage(BankSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Client?> GetAsync(Guid Id)
        {
            if (Id == Guid.Empty)
                return null;

            return await _dbContext.Clients
                .Include(c => c.Accounts)
                .Include(c => c.EmployeeProfile)
                .FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _dbContext.Clients
                .Include(c => c.EmployeeProfile)
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Currency)
                .ToListAsync();
        }
        public async Task<bool> AddAsync(Client client)
        {
            if (client == null)
                return false;

            if (await _dbContext.Clients.AnyAsync(c => c.Id == client.Id || c.PassportNumber == client.PassportNumber))
                return false;

            // Проверяю, есть ли валюта в локальном трекинге
            var currency = _dbContext.Currencies.Local.FirstOrDefault(c => c.Code == "USD");

            // Если нет, ищу в базе данных
            if (currency == null)
                currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == "USD");

            // Если в БД нет, создаю и добавляю в БД
            if (currency == null)
            {
                currency = new Currency("USD", '$');
                _dbContext.Currencies.Add(currency);
            }

            //Если валюта есть в БД, но не не отслеживается - отслеживаю
            else
            {
                if (_dbContext.Entry(currency).State == EntityState.Detached)
                    _dbContext.Currencies.Attach(currency);
            }

            client.Accounts.Add(new Account(client.Id, currency, 0));
            _dbContext.Clients.Add(client);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(Guid idClient, Client upClient)
        {
            if (idClient == Guid.Empty)
                return false;
            if (upClient == null)
                return false;

            var dbClient = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == idClient);

            if (dbClient == null)
                return false;

            _dbContext.Entry(dbClient).CurrentValues.SetValues(upClient);

            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(Guid idClient)
        {
            if (idClient == Guid.Empty)
                return false;
            var dbClient = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == idClient);

            if (dbClient == null)
                return false;

            _dbContext.Clients.Remove(dbClient);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddAccountAsync(Guid clientID, Account account)
        {
            if (account == null)
                return false;

            var dbClient = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientID);

            if (dbClient == null)
                return false;

            var currency = account.Currency;

            var dbCurrency = _dbContext.Currencies.Local.FirstOrDefault(c => c.Code == currency.Code);

            if (dbCurrency == null)
                dbCurrency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == currency.Code);

            if (dbCurrency == null)
            {
                dbCurrency = new Currency(currency.Code, currency.Symbol);
                _dbContext.Currencies.Add(dbCurrency);
            }
            else
            {
                if (_dbContext.Entry(dbCurrency).State == EntityState.Detached)
                    _dbContext.Currencies.Attach(dbCurrency);
            }

            Account addAccount = new Account(dbClient.Id, dbCurrency, account.Amount, account.AccountNumber);

            dbClient.Accounts.Add(addAccount);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAccountAsync(Guid clientID, string accountNumber, Account account)
        {
            if (account == null)
                return false;

            var dbClient = await _dbContext.Clients
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == clientID);

            if (dbClient == null)
                return false;

            var dbAccount = dbClient.Accounts.FirstOrDefault(c => c.AccountNumber == accountNumber);

            if (dbAccount == null)
                return false;



            var dbCurrency = //_dbContext.Currencies.Local.FirstOrDefault(c => c.Code == account.Currency.Code)
                 await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == account.Currency.Code);

            if (dbCurrency == null)
            {
                dbCurrency = new Currency(account.Currency.Code, account.Currency.Symbol);
                _dbContext.Currencies.Add(dbCurrency);
            }
            else
            {
                if (_dbContext.Entry(dbCurrency).State == EntityState.Detached)
                    _dbContext.Currencies.Attach(dbCurrency);
            }

            dbAccount.EditAccount(dbCurrency, account.Amount);

            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAccountAsync(Guid clientID, string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return false;

            var dbClient = await _dbContext.Clients
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == clientID);

            if (dbClient == null)
                return false;

            var dbAccount = dbClient.Accounts.FirstOrDefault(c => c.AccountNumber == accountNumber);

            if (dbAccount == null)
                return false;


            _dbContext.Accounts.Remove(dbAccount);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Client>> GetFilterClientsAsync(ClientFilterDTO filter, int page, int pageSize = 10)
        {
            var query = _dbContext.Clients
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Currency)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.FullName))
                query = query.Where(c => c.FullName.Contains(filter.FullName));

            if (filter.BirthDateFrom.HasValue)
                query = query.Where(c => c.Birthday >= filter.BirthDateFrom);

            if (filter.BirthDateTo.HasValue)
                query = query.Where(c => c.Birthday <= filter.BirthDateTo);

            if (filter.Bonus.HasValue)
                query = query.Where(c => c.Bonus == filter.Bonus);

            if (filter.CountAccounts.HasValue)
                query = query.Where(c => c.Accounts.Count == filter.CountAccounts);

            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(c => c.Email.ToLower().Contains(filter.Email.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
                query = query.Where(c => c.PhoneNumber == filter.PhoneNumber);

            if (!string.IsNullOrWhiteSpace(filter.PassportNumber))
                query = query.Where(c => c.PassportNumber.Trim() == filter.PassportNumber.Trim());

            if (filter.RegisterDateFrom.HasValue)
                query = query.Where(c => c.RegistrationDate >= filter.RegisterDateFrom);

            if (filter.RegisterDateTo.HasValue)
                query = query.Where(c => c.RegistrationDate <= filter.RegisterDateTo);

            int totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(c => c.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Client>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<bool> ExistsAsync(Guid id, string passportNumber)
        {
            return await _dbContext.Clients.AnyAsync(c => c.Id == id || c.PassportNumber == passportNumber);
        }
    }
}
