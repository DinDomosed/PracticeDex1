using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class EmployeeService
    {
        private readonly EmployeeStorage _employeeStorage;

        public EmployeeService(EmployeeStorage employeeStorage)
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

            _employeeStorage.AddEmployeeToStorage(employee);
            return true;
        }
        public bool EditingEmployeeContract(Guid Id, string fullName, string passportNum, DateTime? newDateStartWork = null, DateTime? newDateEndWork = null, decimal? newSalary = null, string? newPost = null)
        {
            if (Id == default(Guid))
                throw new ArgumentNullException("ID не может быть пустым");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentNullException("Имя не может быть пустым", nameof(fullName));
            if (string.IsNullOrWhiteSpace(passportNum))
                throw new PassportNumberNullOrWhiteSpaceException("Некорректный ввод серии и номера паспорта");

            Employee employee = _employeeStorage.AllEmployeeBank.FirstOrDefault(u => u.Key.Equals(Id) && u.Value.FullName == fullName && u.Value.PassportNumber == passportNum).Value;

            if (employee == null)
                throw new EmployeeNotFoundException("Ошибка: Сотрудник не существует");


            var start = newDateStartWork ?? employee.ContractEmployee.StartOfWork;
            var end = newDateEndWork ?? employee.ContractEmployee.EndOfContract;
            var salary = newSalary ?? employee.ContractEmployee.Salary;
            var post = newPost ?? employee.ContractEmployee?.Post;

            employee.SetContract(new EmployeeContract(start, end, salary, post));
            return true;
        }

        public List<Employee> GetFilterEmployee(string? fullName = null, string? passportNumber = null, DateTime? fromThisDateBirthday = null,
            DateTime? beforeThisDateBirthday = null, decimal? fromThisSalary = null, decimal? beforeThisSalary = null)
        {
            var employees = _employeeStorage.AllEmployeeBank.Values;

            if (!string.IsNullOrWhiteSpace(fullName))
                employees = employees.Where(u => u.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(passportNumber))
                employees = employees.Where(u => u.PassportNumber == passportNumber);

            if (fromThisDateBirthday.HasValue)
                employees = employees.Where(u => u.Birthday >= fromThisDateBirthday);

            if (beforeThisDateBirthday.HasValue)
                employees = employees.Where(u => u.Birthday <= beforeThisDateBirthday);

            if(fromThisSalary.HasValue && beforeThisSalary.HasValue)
            {
                employees = employees.Where(u =>
                u.ContractEmployee.Salary >= fromThisSalary.Value &&
                u.ContractEmployee.Salary <= beforeThisSalary.Value);
            }
            else if (fromThisSalary.HasValue)
                employees = employees.Where(u => u.ContractEmployee.Salary >= fromThisSalary.Value);

            else if (beforeThisSalary.HasValue)
                employees = employees.Where(u => u.ContractEmployee.Salary <= beforeThisSalary.Value);

            return employees.ToList();
        }
    }
}
