﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Services;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class EquivalenceTests
    {
        [Fact]
        public void GetHashCodeNecessityPositivTestClients()
        {
            // Arrange
            TestDataGenerator generator = new TestDataGenerator();
            var testdic = generator.GenerateTestDictoinaryClientAndAccount(null, 10);
            Guid guid1 = Guid.NewGuid();

            Client clientTest = new Client(guid1, "Тестовый клиент2", new DateTime(2008, 11, 2), "Clava007@mail.ru", "+7 918 123 36 78", "4324 964623",
                new Account(guid1, new Currency("USD", '$'), 2400));

            testdic.Add(clientTest, clientTest.Accounts);

            Client client = new Client(guid1, "Тестовый клиент2", new DateTime(2008, 11, 2), "Clava007@mail.ru", "+7 918 123 36 78", "4324 964623",
                new Account(guid1, new Currency("USD", '$'), 2400));

            //Act
            bool result = testdic.TryGetValue(client, out List<Account>? account);

            //Assert
            Assert.True(result);
            Assert.NotNull(account);
            Assert.Equal(2400, account[0].Amount);
        }

        [Fact]
        public void GetHashCodeNecessityPositivTestEmployee()
        {
            //Arrange
            TestDataGenerator generator = new TestDataGenerator();
            var testListEmployee = generator.GenerateTestListEmployee(9);

            testListEmployee.Add(new Employee("Ковальчук Диана Андреевна", new DateTime(2003, 12, 31),
                new EmployeeContract(new DateTime(2025, 7, 1), new DateTime(2060, 7, 1), 600, "backend developer"), "4324 666666"));

            Employee lastEmployee = testListEmployee.LastOrDefault();
            Guid guid = lastEmployee.Id;

            Employee testEmployee = new Employee(guid, "Ковальчук Диана Андреевна", new DateTime(2003, 12, 31),
                new EmployeeContract(new DateTime(2025, 7, 1), new DateTime(2060, 7, 1), 600, "backend developer"), "4324 666666");

            //Act
            bool result1 = testListEmployee.Contains(lastEmployee);
            bool result2 = testEmployee.Equals(lastEmployee);

            //Assert
            Assert.True(result1);
            Assert.True(result2);
        }
    }
}
