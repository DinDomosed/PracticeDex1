using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class FakeEmployeeStorage : IEmployeeStorage
    {
        private Dictionary<Guid, Employee> _allEmployeeBank = new Dictionary<Guid, Employee>();
        public IReadOnlyDictionary<Guid, Employee> AllEmployeeBank => _allEmployeeBank;


        public List<Employee> Get(Func<Employee, bool>? predicate = null)
        {
            var employees = _allEmployeeBank.Values.ToList();

            if (predicate != null)
                employees = employees.Where(predicate).ToList();

            return employees;
        }

        public bool Add(Employee employee)
        {
            if (_allEmployeeBank.ContainsKey(employee.Id))
                return false;

            _allEmployeeBank.Add(employee.Id, employee);
            return true;
        }

        public bool Update(Employee employee)
        {
            if (employee == null)
                return false;

            if (!_allEmployeeBank.ContainsKey(employee.Id))
                return false;

            _allEmployeeBank[employee.Id] = employee;
            return true;
        }

        public bool Delete(Employee employee)
        {
            if (!_allEmployeeBank.ContainsKey(employee.Id))
                return false;

            _allEmployeeBank.Remove(employee.Id);
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

        public bool CreateAccount(Guid employeeId, Account account, string email, string phoneNumber)
        {
            if (!_allEmployeeBank.ContainsKey(employeeId))
                return false;
            if (account == null)
                return false;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            _allEmployeeBank[employeeId].CreateClientProfile(email, phoneNumber);
            _allEmployeeBank[employeeId].ClientProfile.Accounts.Add(account);

            return true;
        }
    }
}
