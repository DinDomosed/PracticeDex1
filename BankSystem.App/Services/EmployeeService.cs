using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Common;
using BankSystem.App.DTOs;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeStorage _employeeStorage;

        public EmployeeService(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }
        public Employee? Get(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID");

            return _employeeStorage.Get(id);
        }
        public List<Employee> GetAll()
        {
            return _employeeStorage.GetAll();
        }
        public bool AddEmployee(Employee employee)
        {
            int minAge = 18;
            if (employee.Age < minAge)
                throw new InvalidEmployeeAgeException("Ошибка: Сотрудник должен быть совершеннолетним");

            if (employee.ContractEmployee == null)
                throw new ArgumentNullException("У Сотрудника обязательно должен быть контракт");

            if (string.IsNullOrWhiteSpace(employee.PassportNumber))
                throw new PassportNumberNullOrWhiteSpaceException("Неккоректный ввод серии и номера паспорта");

            return _employeeStorage.Add(employee);
        }
        public bool UpdateEmployee(Guid id, Employee upEmployee)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID");

            if (upEmployee == null)
                throw new ArgumentNullException("Ошибка: Сотрудник не может быть null");

            int minAge = 18;
            if (upEmployee.Age < minAge)
                throw new InvalidEmployeeAgeException("Ошибка: Сотрудник должен быть совершеннолетним");

            if (upEmployee.ContractEmployee == null)
                throw new ArgumentNullException("У Сотрудника обязательно должен быть контракт");

            if (string.IsNullOrWhiteSpace(upEmployee.PassportNumber))
                throw new PassportNumberNullOrWhiteSpaceException("Неккоректный ввод серии и номера паспорта");

            var foundEmployee = _employeeStorage.Get(id);
            if (foundEmployee == null)
                throw new EmployeeNotFoundException("Ошибка: Сотрудник не найден");

            return _employeeStorage.Update(id, upEmployee);
        }
        public bool DeleteEmployee(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID");


            var foundEmployee = _employeeStorage.Get(id);
            if (foundEmployee == null)
                throw new EmployeeNotFoundException("Ошибка: Невозможно удлаить не существующего сотрудника");

            return _employeeStorage.Delete(id);
        }
        public bool UpdateEmployeeContract(Guid id, EmployeeContract newContract)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "Ошибка: Некорректный ID");

            Employee foundEmployee = _employeeStorage.Get(id);

            if (foundEmployee == null)
                throw new EmployeeNotFoundException("Ошибка: Сотрудник не существует");

            return _employeeStorage.UpdateContract(id, newContract);
        }

        public PagedResult<Employee> GetFilterEmployee(EmployeeFilterDTO filter, int page, int pageSize = 10)
        {
            if (page.Equals(default(int)))
                throw new ArgumentException(nameof(page), "Ошибка страницы");

            if (pageSize.Equals(default(int)))
                throw new ArgumentException(nameof(pageSize), "Ошибка: некорректный размер страницы");

            return _employeeStorage.GetFilterEmployees(filter, page, pageSize);
        }

        public bool CreateAccountProfile(Guid employeeId, Account account, string email, string phoneNumber)
        {
            var employee = _employeeStorage.Get(employeeId)
                ?? throw new EmployeeNotFoundException("Ошибка: Клиент не найден");

            if (account == null)
                throw new ArgumentNullException("Ошибка: Счет не может быть null");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(nameof(email), "Ошибка: email введен не корректно");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException(nameof(phoneNumber), "Ошибка: номер телефона введен не корректно");

            return _employeeStorage.CreateClientProfileAndAccount(employeeId, account, email, phoneNumber);
        }

        public bool DeleteAccount(Guid employeeId, Guid idAccount)
        {
            if (employeeId == Guid.Empty)
                throw new ArgumentNullException(nameof(employeeId), "Ошибка: Некорректный ID сотрудника");

            if (idAccount == Guid.Empty)
                throw new ArgumentNullException(nameof(idAccount), "Ошибка: Некорректный ID счета");

            var foundEmployee = _employeeStorage.Get(employeeId)
                ?? throw new EmployeeNotFoundException("Ошибка: Клиент не найден");

            if (foundEmployee.ClientProfile == null)
                throw new ArgumentException(nameof(foundEmployee.ClientProfile), "Ошибка: У данного сотрудника нет счетов");

            var foundAccount = foundEmployee.ClientProfile.Accounts.FirstOrDefault(c => c.Id == idAccount)
                ?? throw new ArgumentException(nameof(idAccount), "Ошибка: у сотрудника нет счета с таким ID");

            return _employeeStorage.DeleteAccount(employeeId, idAccount);
        }
        public bool UpdateAccount(Guid employeeId, Guid idAccount, Account upAccount)
        {
            if (employeeId == Guid.Empty)
                throw new ArgumentNullException(nameof(employeeId), "Ошибка: Некорректный ID сотрудника");

            if (idAccount == Guid.Empty)
                throw new ArgumentNullException(nameof(idAccount), "Ошибка: Некорректный ID счета");

            if (upAccount == null)
                throw new ArgumentNullException(nameof(upAccount), "Ошибка: Обновляемые данные не могут быть null");


            return _employeeStorage.UpdateAccount(employeeId, idAccount, upAccount);
        }
    }
}
