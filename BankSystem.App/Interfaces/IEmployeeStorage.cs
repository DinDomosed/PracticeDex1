using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Common;
using BankSystem.App.DTOs;
using BankSystem.Domain.Models;

namespace BankSystem.App.Interfaces
{
    public interface IEmployeeStorage : IStorage<Employee>
    {
        public bool UpdateContract(Guid employeeId, EmployeeContract newContract);
        public bool CreateClientProfileAndAccount(Guid employeeId, Account account, string email, string phoneNumber);
        public bool DeleteAccount(Guid employeeId, Guid idAccount);
        public bool UpdateAccount(Guid employeeId, Guid idAccount, Account upAccount);
        public PagedResult<Employee> GetFilterEmployees(EmployeeFilterDTO filter, int page, int pageSize);
    }
}
