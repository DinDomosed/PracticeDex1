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
        public Task<bool> UpdateContractAsync(Guid employeeId, EmployeeContract newContract);
        public Task<bool> CreateClientProfileAndAccountAsync(Guid employeeId, Currency currency, string email, string phoneNumber);
        public Task<bool> DeleteAccountAsync(Guid employeeId, Guid idAccount);
        public Task<bool> UpdateAccountAsync(Guid employeeId, Guid idAccount, Account upAccount);
        public Task<PagedResult<Employee>> GetFilterEmployeesAsync(EmployeeFilterDTO filter, int page, int pageSize);
    }
}
