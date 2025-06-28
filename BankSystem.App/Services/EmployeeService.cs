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
            if(employee.Age < minAge)
                throw new InvalidEmployeeAgeException("Ошибка: Сотрудник должен быть совершеннолетним");
            if (employee.ContractEmployee == null)
                throw new ArgumentNullException("У Сотрудника обязательно должен быть контракт");
            if (string.IsNullOrWhiteSpace(employee.PassportNumber))
                throw new PassportNumberNullOrWhiteSpaceException("Неккоректный ввод серии и номера паспорта");

            _employeeStorage.AddEmployeeToStorage(employee);
            return true;
        }
        public bool EditingEmployeeContract(string fullName, string passportNum, DateTime? newDateStartWork, DateTime? newDateEndWork, 
            decimal? newSalary, string? newPost)
        {
            if(string.IsNullOrWhiteSpace(fullName)) 
                throw new ArgumentNullException("Имя не может быть пустым", nameof(fullName));
            if (string.IsNullOrWhiteSpace(passportNum))
                throw new PassportNumberNullOrWhiteSpaceException("Некорректный ввод серии и номера паспорта");

          
                 
            Employee employee = _employeeStorage.AllEmployeeBank.FirstOrDefault(u => u.Value.FullName == fullName && u.Value.PassportNumber == passportNum).Value;
            if (employee == null)
                throw new EmployeeNotFoundException("Ошибка: Сотрудник не существует");

            var start = newDateStartWork ?? employee.ContractEmployee.StartOfWork;
            var end = newDateEndWork ?? employee.ContractEmployee.EndOfContract;
            var salary = newSalary ?? employee.ContractEmployee.Salary;
            var post = newPost ?? employee.ContractEmployee?.Post;

            employee.SetContract(new EmployeeContract(start, end, salary, post));
            return true;
        }

        public List<Employee> GetFilterEmployee(string? fullName, string? passportNumber, DateTime? fromThisDateBirthday,
            DateTime? beforeThisDateBirthday, decimal? fromThisSalary, decimal? beforeThisSalary)
        {
            var employees = _employeeStorage.AllEmployeeBank.Values.AsQueryable();

            if (!string.IsNullOrWhiteSpace(fullName))
                employees = employees.Where(u => u.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(passportNumber))
                employees = employees.Where(u => u.PassportNumber == passportNumber);
            if(fromThisDateBirthday != default(DateTime))
                employees = employees.Where(u => u.Birthday >= fromThisDateBirthday);
            if(beforeThisDateBirthday != default(DateTime))
            {
                if (fromThisDateBirthday.HasValue && fromThisDateBirthday < beforeThisDateBirthday)
                    employees = employees.Where(u => u.Birthday <= beforeThisDateBirthday && u.Birthday >= fromThisDateBirthday);
                else
                    employees = employees.Where(u => u.Birthday <= beforeThisDateBirthday);
            }
            if (fromThisSalary.HasValue)
                employees = employees.Where(u => u.ContractEmployee.Salary >= fromThisSalary);
            if(beforeThisSalary.HasValue)
            {
                if (fromThisSalary.HasValue && fromThisSalary < beforeThisSalary)
                    employees = employees.Where(u => u.ContractEmployee.Salary >= fromThisSalary && u.ContractEmployee.Salary <= beforeThisSalary);
                else
                    employees = employees.Where(u => u.ContractEmployee.Salary <= beforeThisSalary);
            }

            return employees.ToList(); 
        }
    }
}
