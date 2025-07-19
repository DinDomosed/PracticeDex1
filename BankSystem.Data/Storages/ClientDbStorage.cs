using BankSystem.App.Common;
using BankSystem.App.DTOs;
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

        public Client? Get(Guid Id)
        {
            if (Id == Guid.Empty)
                return null;

            return _dbContext.Clients
                .Include(c => c.Accounts)
                .Include(c => c.EmployeeProfile)
                .FirstOrDefault(c => c.Id == Id);
        }

        public List<Client> GetAll()
        {
            return _dbContext.Clients
                .Include(c => c.EmployeeProfile)
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Currency)
                .ToList();
        }
        public bool Add(Client client)
        {
            if (client == null)
                return false;

            if (_dbContext.Clients.Any(c => c.Id == client.Id || c.PassportNumber == client.PassportNumber))
                return false;

            // Проверяю, есть ли валюта в локальном трекинге
            var currency = _dbContext.Currencies.Local.FirstOrDefault(c => c.Code == "USD");

            // Если нет, ищу в базе данных
            if (currency == null)
                currency = _dbContext.Currencies.FirstOrDefault(c => c.Code == "USD");

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
            _dbContext.SaveChanges();
            return true;
        }
        public bool Update(Guid idClient, Client upClient)
        {
            if (idClient == Guid.Empty)
                return false;
            if (upClient == null)
                return false;

            var dbClient = _dbContext.Clients.FirstOrDefault(c => c.Id == idClient);

            if (dbClient == null)
                return false;

            _dbContext.Entry(dbClient).CurrentValues.SetValues(upClient);

            //var entry = _dbContext.Entry(dbClient);

            _dbContext.SaveChanges();
            return true;
        }
        public bool Delete(Guid idClient)
        {
            if (idClient == Guid.Empty)
                return false;
            var dbClient = _dbContext.Clients.FirstOrDefault(c => c.Id == idClient);

            if (dbClient == null)
                return false;

            _dbContext.Clients.Remove(dbClient);
            _dbContext.SaveChanges();
            return true;
        }

        public bool AddAccount(Guid clientID, Account account)
        {
            if (account == null)
                return false;

            var dbClient = _dbContext.Clients.FirstOrDefault(c => c.Id == clientID);

            if (dbClient == null)
                return false;

            var currency = account.Currency;

            var dbCurrency = _dbContext.Currencies.Local.FirstOrDefault(c => c.Code == currency.Code);

            if (dbCurrency == null)
                dbCurrency = _dbContext.Currencies.FirstOrDefault(c => c.Code == currency.Code);

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
            _dbContext.SaveChanges();
            return true;
        }
        public bool UpdateAccount(Guid clientID, string accountNumber, Account account)
        {
            if (account == null)
                return false;

            var dbClient = _dbContext.Clients
                .Include(c => c.Accounts)
                .FirstOrDefault(c => c.Id == clientID);

            if (dbClient == null)
                return false;

            var dbAccount = dbClient.Accounts.FirstOrDefault(c => c.AccountNumber == accountNumber);

            if (dbAccount == null)
                return false;



            var dbCurrency = _dbContext.Currencies.Local.FirstOrDefault(c => c.Code == account.Currency.Code)
                ?? _dbContext.Currencies.FirstOrDefault(c => c.Code == account.Currency.Code);

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

            _dbContext.SaveChanges();
            return true;
        }
        public bool DeleteAccount(Guid clientID, string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return false;

            var dbClient = _dbContext.Clients
                .Include(c => c.Accounts)
                .FirstOrDefault(c => c.Id == clientID);

            if (dbClient == null)
                return false;

            var dbAccount = dbClient.Accounts.FirstOrDefault(c => c.AccountNumber == accountNumber);

            if (dbAccount == null)
                return false;


            _dbContext.Accounts.Remove(dbAccount);
            _dbContext.SaveChanges();
            return true;
        }

        public PagedResult<Client> GetFilterClients(ClientFilterDTO filter, int page, int pageSize = 10)
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

            int totalCount = query.Count();
            var items = query
                .OrderBy(c => c.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Client>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        public bool Exists(Guid id, string passportNumber)
        {
            return _dbContext.Clients.Any(c => c.Id == id || c.PassportNumber == passportNumber);
        }
    }
}
