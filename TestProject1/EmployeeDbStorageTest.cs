using BankSystem.App.DTOs;
using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class EmployeeDbStorageTest
    {
        [Fact]
        public void Add_Test()
        {
            //Arrange 
            BankSystemDbContext dbContext = new BankSystemDbContext();
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);
            TestDataGenerator generator = new TestDataGenerator();

            var employees = generator.GenerateTestListEmployee(10);

            //Act
            bool result = false;
            foreach (var employee in employees)
            {
                result = dbStorage.Add(employee);
            }

            //Assert
            Assert.True(result);
        }


        [Fact]
        public void Update_test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);
            var foundClient = dbStorage.Get(Guid.Parse("33fe71a9-06f1-4028-950b-0469c734638b"));
            var newDateEmployee = new Employee(foundClient.Id, foundClient.FullName, foundClient.Birthday, foundClient.ContractEmployee, "0001 010101");

            //Act
            bool result = dbStorage.Update(Guid.Parse("33fe71a9-06f1-4028-950b-0469c734638b"), newDateEmployee);
            var foundClient2 = dbStorage.Get(Guid.Parse("33fe71a9-06f1-4028-950b-0469c734638b"));

            //Assert
            Assert.True(result);
            Assert.Equal("0001 010101", foundClient2.PassportNumber);
            Assert.NotEqual("1612 633276", foundClient2.PassportNumber);
            Assert.Equal(foundClient.FullName, foundClient2.FullName);
        }

        [Fact]
        public void Delete_test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);

            Guid testId = Guid.NewGuid();
            Employee deletedEmployye = new Employee(testId, "Тестоый сотрудник 007", new DateTime(2000, 1, 1),
                new EmployeeContract(new DateTime(2025, 7, 18), null, 3000, "Backend-developer"), "0020 020202");

            //Act
            var resultAdd = dbStorage.Add(deletedEmployye);
            var foundEmployee = dbStorage.Get(deletedEmployye.Id);
            var resultDelete = dbStorage.Delete(deletedEmployye.Id);

            //Assert
            Assert.True(resultAdd);
            Assert.True(resultDelete);
            Assert.NotNull(foundEmployee);
        }

        [Fact]
        public void UpdateContract_Test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);

            Guid testId = Guid.NewGuid();

            var testContract = new EmployeeContract(new DateTime(2024, 6, 6), null, 2500, "Junior Backend Developer");

            EmployeeContract newDataContract = new EmployeeContract(DateTime.Now, null, 3600, "Middle Backend Developer");


            //Act
            bool resultFirstData = dbStorage.UpdateContract(Guid.Parse("3af9105a-b9e1-497a-aa3d-c237aef22ba9"), testContract);
            bool result = dbStorage.UpdateContract(Guid.Parse("3af9105a-b9e1-497a-aa3d-c237aef22ba9"), newDataContract);
            var foundClient = dbStorage.Get(Guid.Parse("3af9105a-b9e1-497a-aa3d-c237aef22ba9"));

            //Assert
            Assert.True(resultFirstData);
            Assert.True(result);
            Assert.Equal("Middle Backend Developer", foundClient.ContractEmployee.Post);
            Assert.NotEqual(2500, foundClient.ContractEmployee.Salary);
        }

        [Fact]
        public void CreateClientProfileAndAccount_Test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);
            TestDataGenerator generator = new TestDataGenerator();

            var testData_EmailAndPhone = generator.GenerateTestListClients(1).First();

            var EmployeeTest = generator.GenerateTestListEmployee(1).First();
            Currency testCurrency = new Currency("GBP", '£');

            //Act
            bool resultAdd = dbStorage.Add(EmployeeTest);

            bool result = dbStorage.CreateClientProfileAndAccount(EmployeeTest.Id, testCurrency,
                testData_EmailAndPhone.Email, testData_EmailAndPhone.PhoneNumber);

            var foundClient = dbStorage.Get(EmployeeTest.Id);

            //Assert
            Assert.True(result);
            Assert.True(resultAdd);
            Assert.NotNull(foundClient);
            Assert.NotNull(foundClient.ClientProfile);
            Assert.Equal("GBP", foundClient.ClientProfile.Accounts.First().CurrencyCode);

        }

        [Fact]
        public void DeleteAccount_Test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);

            var foundClientProfile = dbStorage.Get(Guid.Parse("141a712b-47bb-4a61-9379-8561605da2d6")).ClientProfile;
            var currency = new Currency("EUR", '€');

            bool resultAdd = dbStorage.CreateClientProfileAndAccount(Guid.Parse("141a712b-47bb-4a61-9379-8561605da2d6"),
                currency, foundClientProfile.Email, foundClientProfile.PhoneNumber);

            //Act
            var foundAcc = foundClientProfile.Accounts.FirstOrDefault(c => c.CurrencyCode == "EUR");

            bool resultDelete = dbStorage.DeleteAccount(Guid.Parse("141a712b-47bb-4a61-9379-8561605da2d6"), foundAcc.Id);

            //Assert
            Assert.NotNull(foundAcc);
            Assert.True(resultAdd);
            Assert.True(resultDelete);
        }

        [Fact]
        public void UpdateAccount_Test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);

            var foundEmployee = dbStorage.Get(Guid.Parse("717f5ba7-5fdb-484d-a948-319143a61ecd"));
            var newDataAccount = new Account(foundEmployee.ClientProfile.Id, new Currency("GBP", '£'), 999);


            //Act
            bool resultAdd = dbStorage.CreateClientProfileAndAccount(foundEmployee.Id, new Currency("EUR", '€'),
                foundEmployee.ClientProfile.Email, foundEmployee.ClientProfile.PassportNumber);

            var foundOldAccount = foundEmployee.ClientProfile.Accounts.FirstOrDefault(c => c.CurrencyCode == "EUR");
            bool resultUpdate = dbStorage.UpdateAccount(foundEmployee.Id, foundOldAccount.Id, newDataAccount);

            //Assert
            Assert.True(resultAdd);
            Assert.NotNull(foundOldAccount);
            Assert.True(resultUpdate);
        }

        [Fact]
        public void GetFilterEmployees_Test()
        {
            //Arrange
            BankSystemDbContext dbContext = new BankSystemDbContext();
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);

            EmployeeFilterDTO filter = new EmployeeFilterDTO()
            {
                SalaryFrom = 5000,
                SalaryTo = 11000

            };

            //Act
            var result = dbStorage.GetFilterEmployees(filter, 1, 10).Items;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
        }
    }
}
