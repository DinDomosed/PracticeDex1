using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public bool UpdateEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException("Ошибка: Сотрудник не может быть null");

            int minAge = 18;
            if (employee.Age < minAge)
                throw new InvalidEmployeeAgeException("Ошибка: Сотрудник должен быть совершеннолетним");

            if (employee.ContractEmployee == null)
                throw new ArgumentNullException("У Сотрудника обязательно должен быть контракт");

            if (string.IsNullOrWhiteSpace(employee.PassportNumber))
                throw new PassportNumberNullOrWhiteSpaceException("Неккоректный ввод серии и номера паспорта");
            

            return _employeeStorage.Update(employee);
        }
        public bool DeleteEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException("Ошибка: Невозможн удалить null сотрудника");

            if (!_employeeStorage.Get().Any(u => u.Id == employee.Id))
                throw new EmployeeNotFoundException("Ошибка: Невозможно удлаить не существующего сотрудника");

            return _employeeStorage.Delete(employee);
        }
        public bool UpdateEmployeeContract(Guid Id, string fullName, string passportNum, DateTime? newDateStartWork = null, DateTime? newDateEndWork = null, decimal? newSalary = null, string? newPost = null)
        {
            if (Id == default(Guid))
                throw new ArgumentNullException("ID не может быть пустым");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentNullException("Имя не может быть пустым", nameof(fullName));
            if (string.IsNullOrWhiteSpace(passportNum))
                throw new PassportNumberNullOrWhiteSpaceException("Некорректный ввод серии и номера паспорта");

            Employee employee = _employeeStorage.Get().FirstOrDefault(u => u.Id == Id && u.FullName == fullName && u.PassportNumber == passportNum);

            if (employee == null)
                throw new EmployeeNotFoundException("Ошибка: Сотрудник не существует");


            var start = newDateStartWork ?? employee.ContractEmployee.StartOfWork;
            var end = newDateEndWork ?? employee.ContractEmployee.EndOfContract;
            var salary = newSalary ?? employee.ContractEmployee.Salary;
            var post = newPost ?? employee.ContractEmployee?.Post;

            _employeeStorage.UpdateContract(employee.Id, new EmployeeContract(start, end, salary, post));
            return true;
        }

        public List<Employee> GetFilterEmployee(string? fullName = null, string? passportNumber = null, DateTime? fromThisDateBirthday = null,
            DateTime? beforeThisDateBirthday = null, decimal? fromThisSalary = null, decimal? beforeThisSalary = null)
        {
            var employees = _employeeStorage.Get();

            if (!string.IsNullOrWhiteSpace(fullName)) 
                employees = employees.Where(u => u.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(passportNumber))
                employees = employees.Where(u => u.PassportNumber == passportNumber).ToList();

            if (fromThisDateBirthday.HasValue)
                employees = employees.Where(u => u.Birthday >= fromThisDateBirthday).ToList();

            if (beforeThisDateBirthday.HasValue)
                employees = employees.Where(u => u.Birthday <= beforeThisDateBirthday).ToList();

            if(fromThisSalary.HasValue && beforeThisSalary.HasValue)
            {
                employees = employees.Where(u =>
                u.ContractEmployee.Salary >= fromThisSalary.Value &&
                u.ContractEmployee.Salary <= beforeThisSalary.Value).ToList();
            }
            else if (fromThisSalary.HasValue)
                employees = employees.Where(u => u.ContractEmployee.Salary >= fromThisSalary.Value).ToList();

            else if (beforeThisSalary.HasValue)
                employees = employees.Where(u => u.ContractEmployee.Salary <= beforeThisSalary.Value).ToList();

            return employees.ToList();
        }

        public bool CreateAccountProfile(Guid employeeId, Account account, string email, string phoneNumber)
        {
            var employee = _employeeStorage.Get(u => u.Id == employeeId).FirstOrDefault()
                ?? throw new EmployeeNotFoundException("Ошибка: Клиент не найден");

            if (account == null)
                throw new ArgumentNullException("Ошибка: Счет не может быть null");

            if(string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email), "Ошибка: email введен не корректно");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber), "Ошибка: номер телефона введен не корректно");

            return _employeeStorage.CreateAccount(employeeId, account, email, phoneNumber);
        }
    }
}
