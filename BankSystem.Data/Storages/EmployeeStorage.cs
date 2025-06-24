using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.Data.Storages
{
    public class EmployeeStorage
    {
        private Dictionary<Guid, Employee> _allEmployeeBank = new Dictionary<Guid, Employee>();
        public IReadOnlyDictionary<Guid, Employee> AllEmployeeBank => _allEmployeeBank;

        public bool AddEmployeeToStorage(Employee employee)
        {
            if (_allEmployeeBank.ContainsKey(employee.Id))
                return false;

            _allEmployeeBank.Add(employee.Id, employee);
            return true;
        }
    }
}
