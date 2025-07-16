using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;
using Bogus;

namespace BankSystem.App.Services
{
    public class TestDataGenerator
    {
        private List<string> _testCodeCurrence = new List<string> { "RUB", "EUR" };
        private List<char> _testSymbolCurrense = new List<char> { '₽', '€' };
        public List<Client> GenerateTestListClients(int count = 1000)
        {
            List<Client> list = new List<Client>();

            //Faker<Account> fakerAcc = new Faker<Account>()
            //    .CustomInstantiator(f => new Account(
            //        GenerateTestCurrency(),
            //        f.Finance.Amount(500, 3000)));

            Faker<Client> fakerClient = new Faker<Client>()
                .CustomInstantiator(f => new Client(
                    f.Name.FullName(),
                    f.Date.Past(90, DateTime.Today.AddYears(-18)),
                    f.Internet.Email(),
                    f.Phone.PhoneNumber("+7 (###) ###-##-##"),
                    f.Random.Replace("#### ######")
                    //fakerAcc.Generate()
                    ));

            for (int i = 0; i < count; i++)
            {
                list.Add(fakerClient.Generate());
            }
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

            Faker<EmployeeContract> fakerContract = new Faker<EmployeeContract>()
                .CustomInstantiator(f => new EmployeeContract(
                    f.Date.Past(5),
                    f.Date.Future(6),
                    f.Random.Decimal(600, 3000),
                    f.Name.JobTitle()));

            Faker<Employee> faker = new Faker<Employee>()
                .CustomInstantiator(f => new Employee(
                    f.Name.FullName(),
                    f.Date.Past(90, DateTime.Today.AddYears(-18)),
                    fakerContract.Generate(),
                    f.Random.Replace("#### ######")));


            for (int i = 0; i < count; i++)
                list.Add(faker.Generate());

            return list;
        }

        public Dictionary<Client, List<Account>> GenerateTestDictoinaryClientAndAccount(List<Client>? clients, int count = 1000)
        {
            Dictionary<Client, List<Account>> dic = new Dictionary<Client, List<Account>>();
            clients ??= GenerateTestListClients(count);

            for (int i = 0; i < clients.Count; i++)
                dic.Add(clients[i], clients[i].Accounts);

            return dic;
        }

        //Вспомогательный метод для гереации 
        private Random Random = new Random();
        protected Currency GenerateTestCurrency()
        {
            int index = Random.Next(0, _testSymbolCurrense.Count());
            char Symbol = _testSymbolCurrense[index];
            string Code = _testCodeCurrence[index];
            return new Currency(Code, Symbol);

        }
    }
}
