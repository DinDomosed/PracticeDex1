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
        public List<Client> GenerateTestListClients(int count = 1000)
        {
            List<Client> list = new List<Client>();
            Faker<Client> fakerClient = new Faker<Client>()
                .CustomInstantiator(f => new Client(
                    f.Name.FullName(),
                    f.Date.Past(90, DateTime.Today.AddYears(-18)),
                    f.Internet.Email(),
                    f.Phone.PhoneNumber("+7 (###) ###-##-##"),
                    f.Random.Replace("#### ######")));

            for (int i = 0; i < count; i++)
            {
                list.Add(fakerClient.Generate());
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
                    fakerContract.Generate()));


            for(int i =0; i< count; i++)
                list.Add(faker.Generate());
            
            return list;
        }
    }
}
