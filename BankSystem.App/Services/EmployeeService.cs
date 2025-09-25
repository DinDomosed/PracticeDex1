using AutoMapper;
using BankSystem.App.Common;
using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeStorage _employeeStorage;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeStorage employeeStorage, IMapper mapper)
        {
            _employeeStorage = employeeStorage;
            _mapper = mapper;
        }
        public async Task<Employee?> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID");

            return await _employeeStorage.GetAsync(id);
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            return await _employeeStorage.GetAllAsync();
        }
        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            int minAge = 18;
            if (employee.Age < minAge)
                throw new InvalidEmployeeAgeException("Ошибка: Сотрудник должен быть совершеннолетним");

            if (employee.ContractEmployee == null)
                throw new ArgumentNullException("У Сотрудника обязательно должен быть контракт");

            if (string.IsNullOrWhiteSpace(employee.PassportNumber))
                throw new PassportNumberNullOrWhiteSpaceException("Неккоректный ввод серии и номера паспорта");

            return await _employeeStorage.AddAsync(employee);
        }
        public async Task<bool> UpdateEmployeeAsync(Guid id, EmployeeDtoForPut employeeDtoForPut)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID");

            var foundEmployee = await _employeeStorage.GetAsync(id);
            if (foundEmployee == null)
                throw new EmployeeNotFoundException("Ошибка: Сотрудник не найден");

            var upEmployee = _mapper.Map(employeeDtoForPut, foundEmployee);

            return await _employeeStorage.UpdateAsync(id, upEmployee);
        }
        public async Task<bool> DeleteEmployeeAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID");


            var foundEmployee = await _employeeStorage.GetAsync(id);
            if (foundEmployee == null)
                throw new EmployeeNotFoundException("Ошибка: Невозможно удлаить не существующего сотрудника");

            return await _employeeStorage.DeleteAsync(id);
        }
        public async Task<bool> UpdateEmployeeContractAsync(Guid id, EmployeeContractDtoForPut dtoUpContract)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID");

            Employee foundEmployee = await _employeeStorage.GetAsync(id);

            if (foundEmployee == null)
                throw new EmployeeNotFoundException("Ошибка: Сотрудник не существует");

            var newContract = foundEmployee.ContractEmployee;

            _mapper.Map(dtoUpContract, newContract);

            return await _employeeStorage.UpdateContractAsync(id, newContract);
        }

        public async Task<PagedResult<EmployeeDtoForGet>> GetFilterEmployeeAsync(EmployeeFilterDTO filter, int page, int pageSize = 10)
        {
            if (page.Equals(default(int)))
                throw new ArgumentException(nameof(page), "Ошибка страницы");

            if (pageSize.Equals(default(int)))
                throw new ArgumentException(nameof(pageSize), "Ошибка: некорректный размер страницы");

            var resultEmployee = await _employeeStorage.GetFilterEmployeesAsync(filter, page, pageSize);

            return _mapper.Map<PagedResult<EmployeeDtoForGet>>(resultEmployee);
        }

        public async Task<bool> CreateAccountProfileAsync(Guid employeeId, Currency currency, string email, string phoneNumber)
        {
            var employee = await _employeeStorage.GetAsync(employeeId)
                ?? throw new EmployeeNotFoundException("Ошибка: Клиент не найден");

            if (currency == null)
                throw new ArgumentNullException("Ошибка: Валюта не может быть null");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(nameof(email), "Ошибка: email введен не корректно");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException(nameof(phoneNumber), "Ошибка: номер телефона введен не корректно");

            return await _employeeStorage.CreateClientProfileAndAccountAsync(employeeId, currency, email, phoneNumber);
        }

        public async Task<bool> DeleteAccountAsync(Guid employeeId, Guid idAccount)
        {
            if (employeeId == Guid.Empty)
                throw new ArgumentNullException(nameof(employeeId), "Ошибка: Некорректный ID сотрудника");

            if (idAccount == Guid.Empty)
                throw new ArgumentNullException(nameof(idAccount), "Ошибка: Некорректный ID счета");

            var foundEmployee = await _employeeStorage.GetAsync(employeeId)
                ?? throw new EmployeeNotFoundException("Ошибка: Клиент не найден");

            if (foundEmployee.ClientProfile == null)
                throw new ArgumentException(nameof(foundEmployee.ClientProfile), "Ошибка: У данного сотрудника нет счетов");

            var foundAccount = foundEmployee.ClientProfile.Accounts.FirstOrDefault(c => c.Id == idAccount)
                ?? throw new ArgumentException(nameof(idAccount), "Ошибка: у сотрудника нет счета с таким ID");

            return await _employeeStorage.DeleteAccountAsync(employeeId, idAccount);
        }
        public async Task<bool> UpdateAccountAsync(Guid employeeId, Guid idAccount, Account upAccount)
        {
            if (employeeId == Guid.Empty)
                throw new ArgumentNullException(nameof(employeeId), "Ошибка: Некорректный ID сотрудника");

            if (idAccount == Guid.Empty)
                throw new ArgumentNullException(nameof(idAccount), "Ошибка: Некорректный ID счета");

            if (upAccount == null)
                throw new ArgumentNullException(nameof(upAccount), "Ошибка: Обновляемые данные не могут быть null");


            return await _employeeStorage.UpdateAccountAsync(employeeId, idAccount, upAccount);
        }
    }
}
