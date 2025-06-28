using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class BankService
    {
        public void CalculateOwnerSalaries(List<Employee> employees, decimal BankProfit, decimal BankExpenses)
        {
            var Owners = employees.Where(e => e.ContractEmployee.Post == "Владелец").ToList();
            if (Owners.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: Владельцы не найдены");
                Console.ResetColor();
            }
            decimal NetProfit = BankProfit - BankExpenses;
            decimal SalaryForOneOwner;
            if (NetProfit < 0)
            {
                Console.WriteLine("У банка нет прибыли . Зарплата владельцам не выдана");
            }
            else if (NetProfit > 0)
            {
                SalaryForOneOwner = (decimal)(NetProfit / Owners.Count);
                foreach (var Owner in Owners)
                {
                    Owner.ContractEmployee.SetSalaryForOwner(SalaryForOneOwner);
                    Console.WriteLine($"Владельцу {Owner.FullName} начислена запралата {SalaryForOneOwner:C}");
                }
            }
        }
        public Employee ConvertClientToEmployee(Client client)
        {
            Console.WriteLine($"Для принятия на работу клиента {client.FullName} необходимо внести следующие данные: ");

            Console.Write($"Введите дату заключения контракта (гггг-мм-дд): ");

            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateStartWork))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверная дата");
                Console.ResetColor();
            }

            Console.Write("Введите дату окончания контракта: ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateEndWork))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверная дата");
                Console.ResetColor();
            }

            Console.Write("Укажите зарплату нового сотрудника:");
            if (!Decimal.TryParse(Console.ReadLine(), out Decimal Salary))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверный формат ввода");
                Console.ResetColor();
            }

            Console.Write("Укажите должность нового сотрудника: ");
            string Post = Console.ReadLine();

            Console.Write("Укажите серию и номер паспорта: ");
            string passportNum = Console.ReadLine();

            Employee employee = new Employee(client.Id, client.FullName, client.Birthday, new EmployeeContract(dateStartWork, dateEndWork, Salary, Post), passportNum);

            return employee;
        }
    }
}
