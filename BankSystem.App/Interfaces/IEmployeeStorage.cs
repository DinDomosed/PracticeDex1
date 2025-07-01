using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IEmployeeStorage : IStorage<Employee>
    {
        public bool UpdateContract(Guid employeeId, EmployeeContract newContract);
        public bool CreateAccount(Guid employeeId, Account account, string email, string phoneNumber);
    }
}
