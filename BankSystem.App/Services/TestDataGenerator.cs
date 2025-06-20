using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Services
{
    public class TestDataGenerator
    {
        private readonly string[] _testNames = { "Диана", "Артём", "Вячеслав", "Анастасия", "Наталья", "Василий", "Рафаэль", "Микеланджело", "Донателло", "Леонардо" };
        private readonly string[] _testSurnames = { "Дюма", "Буонарроти", "Санти", "Барди ", "Данте", "Монро" };
        private readonly string[] _testMiddleName = { "Андреев(ич/на)", "Алексеев(ич/на)", "Вячеславов(ич/на)" };
        private readonly string[] _testDomainsEmail = { "@mail.ru", "@gmail.com", "@yandex.ru", "@outlook.com" };
        private readonly string[] _testPosts = { "Backend developer", "Frontend developer", "DevOps", "Менеджер", "Консультант", "HR", "Бизнес аналитик", "Владелец" };

        Random Random = new Random();

        public List<Client> GenerateTestListClients(int count = 1000)
        {
            List<Client> list = new List<Client>();

            for (int i = 0; i < count; i++)
            {
                string FullName = $"{_testSurnames[Random.Next(0, _testSurnames.Length)]} {_testNames[Random.Next(0, _testNames.Length)]} {_testMiddleName[Random.Next(0, _testMiddleName.Length)]} ";

                list.Add(new Client(FullName,
                    GenerateTestBirhday(),
                    GenerateTestEmailBody() + _testDomainsEmail[Random.Next(0, _testDomainsEmail.Length)],
                    GenerateTestPhoneNumber(),
                    GenerateTestPassportNumber()));
            }
            list.Add(new Client("Неказаков Вячеслав Андреевич ", new DateTime(1980, 11, 2), "Clava007@mail.ru", "+7 918 123 36 78", "4324 964623"));

            return list;
        }

        public Dictionary<string, Client> GenerateTestDictionaryClients(List<Client>? clients, int count = 1000)
        {
            Dictionary<string, Client> dic = new Dictionary<string, Client>();
            clients ??= GenerateTestListClients(count);

            for (int i = 0; i < clients.Count; i++)
            {
                dic.Add(clients[i].PhoneNumber, clients[i]);
            }

            return dic;
        }

        public List<Employee> GenerateTestListEmployee(int count = 1000)
        {
            List<Employee> list = new List<Employee>();
            for (int i = 0; i < count; i++)
            {
                DateTime startWork = GenerateDateStartContract();
                string FullName = $"{_testSurnames[Random.Next(0, _testSurnames.Length)]} {_testNames[Random.Next(0, _testNames.Length)]} {_testMiddleName[Random.Next(0, _testMiddleName.Length)]} ";
                string Post = _testPosts[Random.Next(0, _testPosts.Length)];

                list.Add(new Employee(FullName, GenerateTestBirhday(),
                    new EmployeeContract(startWork, GenerateDateEndContract(startWork), Random.Next(500, 2000), Post)));
            }
            return list;
        }


        //Вспомогательные методы для генераций

        protected string GenerateTestPhoneNumber()
        {
            int part1 = Random.Next(100, 999);
            int part2 = Random.Next(10, 99);
            int part3 = Random.Next(10, 99);

            return $"+7 918 {part1}-{part2}-{part3}";
        }

        protected string GenerateTestEmailBody()
        {
            char[] chars = {'q','w','e','r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f',
                'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b','n', 'm'};
            int maxLength = chars.Length;
            string body = "";

            for (int i = 0; i < Random.Next(5, maxLength); i++)
            {
                body += chars[Random.Next(0, chars.Length)];
            }

            return body.Trim();
        }
        protected string GenerateTestPassportNumber()
        {
            int part1 = Random.Next(1000, 9999);
            int part2 = Random.Next(100000, 999999);

            return $"{part1} {part2}";
        }
        protected DateTime GenerateTestBirhday(int minAge = 18, int maxAge = 99)
        {
            DateTime today = DateTime.Today;
            int Age = Random.Next(minAge, maxAge + 1);

            DateTime BD = today.AddYears(-Age);
            BD = BD.AddDays(-Random.Next(1, 365));

            return BD;
        }

        protected DateTime GenerateDateStartContract(int maximumAgeDifferenceFromToday = 5)
        {
            DateTime today = DateTime.Today;
            int AgeWork = Random.Next(1, maximumAgeDifferenceFromToday);

            DateTime StartWork = today.AddYears(AgeWork);
            StartWork = StartWork.AddDays(-Random.Next(0, 365));

            return StartWork;
        }
        protected DateTime GenerateDateEndContract(DateTime StartWork, int minAgeContract = 6)
        {
            DateTime today = DateTime.Today;
            DateTime EndWork = StartWork.AddYears(+Random.Next(1, minAgeContract));

            return EndWork;
        }
    }
}
