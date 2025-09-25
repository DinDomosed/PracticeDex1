using BankSystem.App.Common;
using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
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

        public async Task<Employee?> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            return await _dbContext.Employees
                .Include(c => c.ContractEmployee)
                .Include(c => c.ClientProfile)
                .ThenInclude(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            return await _dbContext.Employees
                .Include(c => c.ContractEmployee)
                .Include(c => c.ClientProfile)
                .ThenInclude(c => c.Accounts)
                .ToListAsync();
        }
        public async Task<bool> AddAsync(Employee employee)
        {
            if (await _dbContext.Employees.AnyAsync(c => c.Id == employee.Id || c.PassportNumber == employee.PassportNumber))
                return false;

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(Guid id, Employee upEmployee)
        {
            if (id == Guid.Empty)
                return false;

            var dbEmp = await _dbContext.Employees.FirstOrDefaultAsync(c => c.Id == id);

            if (dbEmp == null)
                return false;

            _dbContext.Employees.Entry(dbEmp).CurrentValues.SetValues(upEmployee);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            var dbEmp = await _dbContext.Employees.FirstOrDefaultAsync(c => c.Id == id);

            if (dbEmp == null)
                return false;

            _dbContext.Employees.Remove(dbEmp);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateContractAsync(Guid employeeId, EmployeeContract newContract)
        {
            if (newContract == null)
                return false;

            if (employeeId == Guid.Empty)
                return false;

            var dbEmployee = await _dbContext.Employees.FirstOrDefaultAsync(c => c.Id == employeeId);

            if (dbEmployee == null)
                return false;

            var dbEmployeeContract = await _dbContext.EmployeeContracts.FirstOrDefaultAsync(c => c.EmployeeId == dbEmployee.Id);

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
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateClientProfileAndAccountAsync(Guid employeeId, Currency currency, string email, string phoneNumber)
        {
            if (employeeId == Guid.Empty)
                return false;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var dbEmployee = await _dbContext.Employees.FirstOrDefaultAsync(c => c.Id == employeeId);

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
                dbCurrency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == currency.Code);

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

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccountAsync(Guid employeeId, Guid idAccount)
        {
            if (employeeId == Guid.Empty)
                return false;

            if (idAccount == Guid.Empty)
                return false;

            var dbEmployee = await _dbContext.Employees
                .Include(c => c.ClientProfile)
                .ThenInclude(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == employeeId);

            if (dbEmployee == null)
                return false;

            if (dbEmployee.ClientProfile == null)
                return false;

            var dbAccount = dbEmployee.ClientProfile.Accounts.FirstOrDefault(c => c.Id == idAccount);

            if (dbAccount == null)
                return false;

            dbEmployee.ClientProfile.Accounts.Remove(dbAccount);
            _dbContext.Accounts.Remove(dbAccount);
            await _dbContext.SaveChangesAsync();
            return true;

        }
        public async Task<bool> UpdateAccountAsync(Guid employeeId, Guid idAccount, Account upAccount)
        {
            if (employeeId == Guid.Empty)
                return false;
            if (idAccount == Guid.Empty)
                return false;
            if (upAccount == null)
                return false;

            var dbEmployee = await _dbContext.Employees
                .Include(c => c.ClientProfile)
                .ThenInclude(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == employeeId);

            if (dbEmployee == null)
                return false;

            var dbAccount = dbEmployee.ClientProfile.Accounts.FirstOrDefault(c => c.Id == idAccount);

            if (dbAccount == null)
                return false;

            var dbCurrency = _dbContext.Currencies.Local.FirstOrDefault(c => c.Code == upAccount.CurrencyCode);

            if (dbCurrency == null)
                await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == upAccount.CurrencyCode);

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
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Employee>> GetFilterEmployeesAsync(EmployeeFilterDTO filter, int page, int pageSize)
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

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Employee>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<bool> ExistsAsync(Guid id, string passportNumber)
        {
            return await _dbContext.Employees.AnyAsync(c => c.Id == id || c.PassportNumber == passportNumber);
        }
    }
}
