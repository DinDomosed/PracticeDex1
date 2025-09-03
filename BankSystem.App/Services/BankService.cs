using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Events;
using BankSystem.App.Exceptions;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class BankService
    {
        protected readonly List<Person> BlackList = new List<Person>();
        private readonly EmployeeService _employeeService;
        public BankService(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        public event EventHandler<NoProfitEventArgs>? ThereIsNoProfit;
        public event EventHandler<OwnersSalariesEventArgs>? ThereIsSalariesOwners;

        protected virtual void OnThereIsNoProfit(decimal netProfit)
        {
            ThereIsNoProfit?.Invoke(this, new NoProfitEventArgs(netProfit));
        }
        protected virtual void OnThereIsSalariesOwners(Employee owner, decimal salary)
        {
            ThereIsSalariesOwners?.Invoke(this, new OwnersSalariesEventArgs(owner, salary));
        }

        public bool AddBonus<T>(T person, decimal amount) where T : Person
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person), "Ошибка: Persone не может быть null");
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Ошибка: Сумма бонуса не может быть меньше 0");

            person.AddBonus(amount);
            return true;
        }
        public bool AddToBlackList<T> (T person) where T : Person
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person), "Ошибка: Невозможно null person добавить в черный список");
            if(BlackList.Contains(person))
                throw new InvalidOperationException("Ошибка: Person уже есть в BlackList");

            BlackList.Add(person);
            return true;
        }
        public bool IsPersonInBlackList<T> (T person) where T : Person
        {
            if (person == null)
                throw new ArgumentNullException("Ошибка: Невозможно проверить null");

            return BlackList.Contains(person);
        }

        public void CalculateOwnerSalaries(List<Employee> employees, decimal bankProfit, decimal bankExpenses)
        {
            
            var Owners = employees.Where(e => e.ContractEmployee.Post == "Владелец").ToList();
            if (Owners.Count == 0)
            {
                throw new EmployeeNotFoundException("Ошибка: Владельцы не найдены");           
            }

            decimal NetProfit = bankProfit - bankExpenses;
            decimal SalaryForOneOwner;

            if (NetProfit < 0)
            {
                OnThereIsNoProfit(NetProfit);
            }

            else if (NetProfit > 0)
            {
                SalaryForOneOwner = (decimal)(NetProfit / Owners.Count);
                foreach (var Owner in Owners)
                {
                    Owner.ContractEmployee.SetSalaryForOwner(SalaryForOneOwner);
                    OnThereIsSalariesOwners(Owner, SalaryForOneOwner);
                }
            }
        }
        public Employee ConvertClientToEmployeeAndAddToStorage(Client client, DateTime dateStartWork, DateTime dateEndWork, decimal salary, string post)
        {
            if(client == null)
                throw new ArgumentNullException(nameof(client), "Ошибка: Клиент не может быть null");
            if (dateStartWork.Equals(default(DateTime)) || dateEndWork.Equals(default(DateTime)))
                throw new ArgumentNullException("Ошибка: Некорректный ввод дат");
            if(salary <= 0)
                throw new ArgumentNullException("Ошибка: Некорректный ввод зарплаты");
            if(string.IsNullOrWhiteSpace(post))
                throw new ArgumentNullException(nameof(post));

            Employee employee = new Employee(client.Id, client.FullName, client.Birthday, new EmployeeContract(dateStartWork, dateEndWork, salary, post), client.PassportNumber);
            _employeeService.AddEmployeeAsync(employee);

            return employee;
        }
    }
}
