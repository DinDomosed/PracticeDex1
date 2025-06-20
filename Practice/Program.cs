using System.Runtime.CompilerServices;
using BankSystem.Domain.Models;
using BankSystem.App.Services;
using System.Diagnostics;
namespace Practice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int countGenerate = 1000;
            var generator = new TestDataGenerator();

            var listClients = generator.GenerateTestListClients();
            var dictionaryClients = generator.GenerateTestDictionaryClients(listClients, countGenerate);
            var listEmployee = generator.GenerateTestListEmployee(countGenerate);

            //Замер времени выполнения поиска по списку
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Client foundClient = listClients.Find(c => c.PhoneNumber == "+7 918 123 36 78");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + " мс");
            Console.WriteLine(stopwatch.ElapsedTicks + "тиков");
            Console.WriteLine(foundClient.ToString());


            //Замер времени выполнения поиска по словарю

            stopwatch.Restart();
            Client foundClientInDic = dictionaryClients["+7 918 123 36 78"];
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + " мс");
            Console.WriteLine(stopwatch.ElapsedTicks + "тиков");
            Console.WriteLine(foundClientInDic.ToString());

            //Выборка клиентов , чей возраст ниже определенного значения

            var clientAge = listClients
                .Where(c => c.Age < 21)
                .ToList();
            for (int i = 0; i < clientAge.Count; i++)
            {
                Console.WriteLine(clientAge[i].ToString() + i);
            }

            //Поиск сотрудника с минимальной зп

            var minSalary = listEmployee.MinBy(c => c.ContractEmployee.Salary);
            Console.WriteLine(new string('-', 10));
            Console.WriteLine("Сотрудник с минимальной зарплатой:\n\n {0}", minSalary);

            //Сравнение скорости по словарю между поиском по LastOrDefault и ключу
            stopwatch.Restart();
            var lastClient = dictionaryClients.LastOrDefault();
            if (!lastClient.Equals(default(KeyValuePair<string, Client>)))
                Console.WriteLine(lastClient.ToString());
            stopwatch.Stop();

            Console.WriteLine($"Итоги поиска последнего элемента с помощью метода LINQ, LastOrDefault:\n\n" +
                    $"Время поиска - {stopwatch.ElapsedMilliseconds} миллисекунд\n" +
                    $"{stopwatch.ElapsedTicks} тиков\n\n");


            stopwatch.Restart();
            if (dictionaryClients.TryGetValue(lastClient.Key, out Client value))
                Console.WriteLine(value.ToString());
            stopwatch.Stop();

            Console.WriteLine($"Итоги поиска по ключу : \n\n" +
                   $"Время поиска - {stopwatch.ElapsedMilliseconds} mc\n" +
                   $"{stopwatch.ElapsedTicks} тиков");

            //Итог, поиск по ключу быстрее

        }
        static void UpdateContractEmployee(Employee employee)
        {
            Console.Write("Введите дату заключения контракта (гггг-мм-дд): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime newDateStart))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверная дата");
                Console.ResetColor();
                return;
            }

            Console.Write("Введите дату окончания контракта (гггг-мм-дд): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime newDateEnd))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: неверная дата");
                Console.ResetColor();
                return;
            }

            Console.Write("Укажите зарплату сотрудника: ");
            if (!Decimal.TryParse(Console.ReadLine(), out decimal newSalary))
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
        static void UpdateCurrency(ref Currency currency)
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
