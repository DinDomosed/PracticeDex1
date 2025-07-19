using BankSystem.App.Common;
using BankSystem.App.DTOs;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;


namespace BankSystem.Data.Storages
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private Dictionary<Guid, Employee> _allEmployeeBank = new Dictionary<Guid, Employee>();
        public IReadOnlyDictionary<Guid, Employee> AllEmployeeBank => _allEmployeeBank;


        public Employee? Get(Guid Id)
        {
            if (Id == Guid.Empty)
                return null;

            if (!_allEmployeeBank.ContainsKey(Id))
                return null;

            return _allEmployeeBank[Id];
        }
        public List<Employee> GetAll()
        {
            return _allEmployeeBank.Values.ToList();
        }

        public bool Add(Employee employee)
        {
            if (_allEmployeeBank.ContainsKey(employee.Id))
                return false;

            _allEmployeeBank.Add(employee.Id, employee);
            return true;
        }

        public bool Update(Guid Id, Employee upEmployee)
        {
            if (Id == Guid.Empty)
                return false;
            if (upEmployee == null)
                return false;

            if (!_allEmployeeBank.ContainsKey(Id))
                return false;

            _allEmployeeBank[Id] = upEmployee;
            return true;
        }

        public bool Delete(Guid Id)
        {
            if (Id == Guid.Empty)
                return false;

            if (!_allEmployeeBank.ContainsKey(Id))
                return false;

            _allEmployeeBank.Remove(Id);
            return true;
        }

        public bool UpdateContract(Guid employeeId, EmployeeContract newEmployeeContract)
        {
            if (!_allEmployeeBank.ContainsKey(employeeId))
                return false;

            if (newEmployeeContract == null)
                return false;

            _allEmployeeBank[employeeId].SetContract(newEmployeeContract);
            return true;
        }

        public bool CreateClientProfileAndAccount(Guid employeeId, BankSystem.Domain.Models.Currency currency, string email, string phoneNumber)
        {
            if (!_allEmployeeBank.TryGetValue(employeeId, out var employee))
                return false;
            if (currency == null)
                return false;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            if (employee.ClientProfile != null)
                return false;

            Account account = new Account(employeeId, currency, 0);

            _allEmployeeBank[employeeId].CreateClientProfile(email, phoneNumber);
            _allEmployeeBank[employeeId].ClientProfile.Accounts.Add(account);

            return true;
        }

        public bool DeleteAccount(Guid employeeId, Guid idAccount)
        {
            if (employeeId == Guid.Empty || idAccount == Guid.Empty)
                return false;

            if (!_allEmployeeBank.ContainsKey(employeeId))
                return false;

            var foundEmployee = _allEmployeeBank[employeeId];

            if (foundEmployee.ClientProfile == null)
                return false;

            Account foundAccount = foundEmployee.ClientProfile.Accounts.FirstOrDefault(c => c.Id == idAccount);

            if (foundAccount == null)
                return false;

            foundEmployee.ClientProfile.Accounts.Remove(foundAccount);
            return true;
        }

        public bool UpdateAccount(Guid employeeId, Guid idAccount, Account upAccount)
        {
            if (employeeId == Guid.Empty || idAccount == Guid.Empty)
                return false;

            if (upAccount == null)
                return false;

            if (!_allEmployeeBank.ContainsKey(employeeId))
                return false;

            var foundEmployee = _allEmployeeBank[employeeId];

            if (foundEmployee.ClientProfile == null)
                return false;
            var foundAccount = foundEmployee.ClientProfile.Accounts.FirstOrDefault(c => c.Id == idAccount);

            if (foundAccount == null)
                return false;

            foundAccount.EditAccount(upAccount.Currency, upAccount.Amount);
            return true;
        }

        public PagedResult<Employee> GetFilterEmployees(EmployeeFilterDTO filter, int page, int pageSize)
        {
            IEnumerable<Employee> employees = _allEmployeeBank.Values;


            if (!string.IsNullOrWhiteSpace(filter.FullName))
                employees = employees.Where(c => c.FullName.ToLower() == filter.FullName.ToLower());

            if (filter.BirthDateFrom.HasValue)
                employees = employees.Where(c => c.Birthday >= filter.BirthDateFrom);

            if (filter.BirthDateTo.HasValue)
                employees = employees.Where(c => c.Birthday <= filter.BirthDateTo);

            if (filter.Bonus.HasValue)
                employees = employees.Where(c => c.Bonus == filter.Bonus);

            if (!string.IsNullOrWhiteSpace(filter.PassportNumber))
                employees = employees.Where(c => c.PassportNumber == filter.PassportNumber);

            if (filter.StartWorkDateFrom.HasValue)
                employees = employees.Where(c => c.ContractEmployee.StartOfWork >= filter.StartWorkDateFrom);

            if (filter.EndContractDateTo.HasValue)
                employees = employees.Where(c => c.ContractEmployee.EndOfContract <= filter.EndContractDateTo);

            if (filter.SalaryFrom.HasValue)
                employees = employees.Where(c => c.ContractEmployee.Salary >= filter.SalaryFrom);

            if (filter.SalaryTo.HasValue)
                employees = employees.Where(c => c.ContractEmployee.Salary <= filter.SalaryTo);

            if (!string.IsNullOrWhiteSpace(filter.Post))
                employees = employees.Where(c => c.ContractEmployee.Post.ToLower().Contains(filter.Post.ToLower()));

            int totalCount = employees.Count();

            var items = employees
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
            return _allEmployeeBank.Any(c => c.Value.Id == id || c.Value.PassportNumber == passportNumber);
        }
    }
}
