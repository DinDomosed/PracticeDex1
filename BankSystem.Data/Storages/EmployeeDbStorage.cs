using BankSystem.App.Common;
using BankSystem.App.DTOs;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Storages
{
    public class EmployeeDbStorage : IEmployeeStorage
    {
        private readonly BankSystemDbContext _dbContext;
        public EmployeeDbStorage(BankSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Employee? Get(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return _dbContext.Employees
                .Include(c => c.ContractEmployee)
                .Include(c => c.ClientProfile)
                .ThenInclude(c => c.Accounts)
                .FirstOrDefault(c => c.Id == id);
        }
        public List<Employee> GetAll()
        {
            return _dbContext.Employees
                .Include(c => c.ContractEmployee)
                .Include(c => c.ClientProfile)
                .ThenInclude(c => c.Accounts)
                .ToList();
        }
        public bool Add(Employee employee)
        {
            if (_dbContext.Employees.Any(c => c.Id == employee.Id || c.PassportNumber == employee.PassportNumber))
                return false;

            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
            return true;
        }
        public bool Update(Guid id, Employee upEmployee)
        {
            if (id == Guid.Empty)
                return false;

            var dbEmp = _dbContext.Employees.FirstOrDefault(c => c.Id == id);

            if (dbEmp == null)
                return false;

            _dbContext.Employees.Entry(dbEmp).CurrentValues.SetValues(upEmployee);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Delete(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            var dbEmp = _dbContext.Employees.FirstOrDefault(c => c.Id == id);

            if (dbEmp == null)
                return false;

            _dbContext.Employees.Remove(dbEmp);
            _dbContext.SaveChanges();
            return true;
        }

        public bool UpdateContract(Guid employeeId, EmployeeContract newContract)
        {
            if (newContract == null)
                return false;

            if (employeeId == Guid.Empty)
                return false;

            var dbEmployee = _dbContext.Employees.FirstOrDefault(c => c.Id == employeeId);

            if (dbEmployee == null)
                return false;

            var dbEmployeeContract = _dbContext.EmployeeContracts.FirstOrDefault(c => c.EmployeeId == dbEmployee.Id);

            if (dbEmployeeContract == null)
                return false;

            if (dbEmployeeContract.EmployeeId != dbEmployee.Id)
                return false;

            var entry = _dbContext.Entry(dbEmployeeContract);
            entry.Property(c => c.StartOfWork).IsModified = true;
            entry.Property(c => c.EndOfContract).IsModified = true;
            entry.Property(c => c.Salary).IsModified = true;
            entry.Property(c => c.Post).IsModified = true;

            dbEmployee.SetContract(newContract);
            _dbContext.SaveChanges();
            return true;
        }

        public bool CreateClientProfileAndAccount(Guid employeeId, Currency currency, string email, string phoneNumber)
        {
            if (employeeId == Guid.Empty)
                return false;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var dbEmployee = _dbContext.Employees.FirstOrDefault(c => c.Id == employeeId);

            if (dbEmployee == null)
                return false;

            var clientProf = dbEmployee.ClientProfile;

            if (clientProf == null)
            {
                dbEmployee.CreateClientProfile(email, phoneNumber);
                clientProf = dbEmployee.ClientProfile;

                if (clientProf == null)
                    return false;
                //Тут добавляем в БД, в том случае, если впервые создаем сотруднику клентский профиль
                else
                    _dbContext.Clients.Add(clientProf);
            }



            if (currency == null)
                return false;

            var dbCurrency = _dbContext.Currencies.Local.FirstOrDefault(c => c.Code == currency.Code);

            if (dbCurrency == null)
                dbCurrency = _dbContext.Currencies.FirstOrDefault(c => c.Code == currency.Code);

            if (dbCurrency == null)
            {
                _dbContext.Currencies.Add(currency);
                dbCurrency = currency;
            }
            else
            {
                if (_dbContext.Entry(dbCurrency).State == EntityState.Detached)
                    _dbContext.Currencies.Attach(dbCurrency);
            }



            Account account = new Account(clientProf.Id, dbCurrency, 0);

            clientProf.Accounts.Add(account);

            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteAccount(Guid employeeId, Guid idAccount)
        {
            if (employeeId == Guid.Empty)
                return false;

            if (idAccount == Guid.Empty)
                return false;

            var dbEmployee = _dbContext.Employees
                .Include(c => c.ClientProfile)
                .ThenInclude(c => c.Accounts)
                .FirstOrDefault(c => c.Id == employeeId);

            if (dbEmployee == null)
                return false;

            if (dbEmployee.ClientProfile == null)
                return false;

            var dbAccount = dbEmployee.ClientProfile.Accounts.FirstOrDefault(c => c.Id == idAccount);

            if (dbAccount == null)
                return false;

            dbEmployee.ClientProfile.Accounts.Remove(dbAccount);
            _dbContext.Accounts.Remove(dbAccount);
            _dbContext.SaveChanges();
            return true;

        }
        public bool UpdateAccount(Guid employeeId, Guid idAccount, Account upAccount)
        {
            if (employeeId == Guid.Empty)
                return false;
            if (idAccount == Guid.Empty)
                return false;
            if (upAccount == null)
                return false;

            var dbEmployee = _dbContext.Employees
                .Include(c => c.ClientProfile)
                .ThenInclude(c => c.Accounts)
                .FirstOrDefault(c => c.Id == employeeId);

            if (dbEmployee == null)
                return false;

            var dbAccount = dbEmployee.ClientProfile.Accounts.FirstOrDefault(c => c.Id == idAccount);

            if (dbAccount == null)
                return false;

            var dbCurrency = _dbContext.Currencies.Local.FirstOrDefault(c => c.Code == upAccount.CurrencyCode);

            if (dbCurrency == null)
                _dbContext.Currencies.FirstOrDefault(c => c.Code == upAccount.CurrencyCode);

            if (dbCurrency == null)
            {
                dbCurrency = new Currency(upAccount.CurrencyCode, upAccount.Currency.Symbol);
            }

            else
            {
                if (_dbContext.Entry(dbCurrency).State == EntityState.Detached)
                    _dbContext.Currencies.Attach(dbCurrency);
            }


            dbAccount.EditAccount(dbCurrency, upAccount.Amount);
            _dbContext.SaveChanges();
            return true;
        }

        public PagedResult<Employee> GetFilterEmployees(EmployeeFilterDTO filter, int page, int pageSize)
        {
            var query = _dbContext.Employees
                .Include(c => c.ClientProfile)
                .Include(c => c.ContractEmployee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.FullName))
                query = query.Where(c => c.FullName.ToLower() == filter.FullName.ToLower());

            if (filter.BirthDateFrom.HasValue)
                query = query.Where(c => c.Birthday >= filter.BirthDateFrom);

            if (filter.BirthDateTo.HasValue)
                query = query.Where(c => c.Birthday <= filter.BirthDateTo);

            if (filter.Bonus.HasValue)
                query = query.Where(c => c.Bonus == filter.Bonus);

            if (!string.IsNullOrWhiteSpace(filter.PassportNumber))
                query = query.Where(c => c.PassportNumber == filter.PassportNumber);

            if (filter.StartWorkDateFrom.HasValue)
                query = query.Where(c => c.ContractEmployee != null && c.ContractEmployee.StartOfWork >= filter.StartWorkDateFrom);

            if (filter.EndContractDateTo.HasValue)
                query = query.Where(c => c.ContractEmployee != null && c.ContractEmployee.EndOfContract <= filter.EndContractDateTo);

            if (filter.SalaryFrom.HasValue)
                query = query.Where(c => c.ContractEmployee != null && c.ContractEmployee.Salary >= filter.SalaryFrom);

            if (filter.SalaryTo.HasValue)
                query = query.Where(c => c.ContractEmployee != null && c.ContractEmployee.Salary <= filter.SalaryTo);

            if (!string.IsNullOrWhiteSpace(filter.Post))
                query = query.Where(c => c.ContractEmployee != null && c.ContractEmployee.Post.ToLower().Contains(filter.Post.ToLower()));

            int totalCount = query.Count();

            var items = query
                .OrderBy(c => c.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Employee>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        public bool Exists(Guid id, string passportNumber)
        {
            return _dbContext.Employees.Any(c => c.Id == id || c.PassportNumber == passportNumber);
        }
    }
}
