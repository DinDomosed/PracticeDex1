using System.Runtime.CompilerServices;
using BankSystem.Domain.Models;
using BankSystem.App.Services;
namespace Practice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new string('-',20));
           
            Employee employee = new Employee("Ковальчук Диана Андреевна", new DateTime(2003, 12, 31), new EmployeeContract(DateTime.Now, new DateTime(2040, 1, 13), 500, "Backend developer"));
            Console.WriteLine(employee.ToString() + '\n');
            
            Console.WriteLine(new string('-', 20));
           
            Client client = new Client("Неказаков Вячеслав Андреевич ", new DateTime(1980, 11, 2), "Clava007@mail.ru", "+7 918 123 36 78", "4324 964623");
            Console.WriteLine(client.ToString());
            
            Console.WriteLine(new string('-', 20));

            // Тестирование метода для преобразования Клиента в сотрудника....
            BankService bankService = new BankService();
            Employee newEmployee = bankService.ConvertClientToEmployee(client);
            Console.WriteLine('\n' + new string('=', 20));
            Console.WriteLine(newEmployee.ToString());
            Console.WriteLine('\n' + new string('=', 20));

            List<Employee> employees = new List<Employee>
            {
                new Employee("Соколов Андрей Андреевич",  new DateTime(1980, 4, 4), new EmployeeContract(DateTime.Now, new DateTime(2100, 1, 13), 0, "Владелец")),
                new Employee("Соколов Василий Витальевич",  new DateTime(1983, 12, 6), new EmployeeContract(DateTime.Now, new DateTime(2100, 1, 13), 0, "Владелец")),
            };
            employees.Add(newEmployee);
            employees.Add(employee);

            // Тестирование метода для расчета запрлаты Владельцев....
            bankService.CalculateOwnerSalaries(employees, 13000000, 9000000);

            Console.WriteLine('\n' + new string('=', 20) + '\n');
            foreach (var empl  in employees)
            { 
                Console.WriteLine(empl.ToString()) ;
                Console.WriteLine('\n' + new string('=', 20)+'\n');
            }


            ////Тестирование метода для обновление контракта с существующим сотрудником ......

            //UpdateContractEmployee(employee);
            //Console.WriteLine(employee.ContractEmployee.ToString());

            //Console.WriteLine(new string('=', 20) + '\n');

            //Currency currency = new Currency("USD", '$');

            ////Тестирование метода для обновления валюты .....

            //Console.WriteLine(currency.ToString() + "\n");
            //UpdateCurrency(ref currency);
            //Console.WriteLine(currency.ToString());
        }
        static void UpdateContractEmployee (Employee employee)
        {
            Console.Write("Введите дату заключения контракта (гггг-мм-дд): ");
            if(!DateTime.TryParse(Console.ReadLine(), out DateTime newDateStart))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверная дата");
                Console.ResetColor();
                return;
            }

            Console.Write("Введите дату окончания контракта (гггг-мм-дд): ");
            if(!DateTime.TryParse(Console.ReadLine(), out DateTime newDateEnd))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверная дата");
                Console.ResetColor();
                return;
            }

            Console.Write("Укажите зарплату сотрудника: ");
            if(!Decimal.TryParse(Console.ReadLine(), out decimal newSalary))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверный формат ввода");
                Console.ResetColor();
            }

            Console.Write("Введите должность сотрудника: ");
            string newPost = Console.ReadLine();

            EmployeeContract newContract = new EmployeeContract(newDateStart, newDateEnd, newSalary, newPost);
            employee.SetContract(newContract);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Контракт для сотрудника {employee.FullName},  успешно обновлен!\n");
            Console.ResetColor();
        }
        static void UpdateCurrency (ref Currency currency)
        {
            Console.Write("Введите код новой валюты: ");
            string newCode = Console.ReadLine();

            Console.Write("Введите символ для обозначения новой валюты: ");
            char newSymbol = Convert.ToChar(Console.ReadLine());

            currency = new Currency(newCode, newSymbol);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Валюта успешно обновлена \n");
            Console.ResetColor();
        }
    }
}
