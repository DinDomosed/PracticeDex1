using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using BankSystem.App.Services;
using BankSystem.Data;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task Add_Test()
        {
            //Arrange 
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            BankSystemDbContext dbContext = new BankSystemDbContext(options);
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);
            TestDataGenerator generator = new TestDataGenerator();

            var employees = generator.GenerateTestListEmployee(10);

            //Act
            bool result = false;
            foreach (var employee in employees)
            {
                result = await dbStorage.AddAsync(employee);
            }

            //Assert
            Assert.True(result);
        }


        [Fact]
        public async Task Update_test()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using BankSystemDbContext dbContext = new BankSystemDbContext(options);
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);


            var oldEmployee = new Employee(Guid.Parse("33fe71a9-06f1-4028-950b-0469c734638b"),"Тествовый сотрудник", new DateTime(2000, 1, 1),
                new EmployeeContract(new DateTime(2025, 1, 1), null, 6000, "Backend Developer"), "1612 633276");

            await dbStorage.AddAsync(oldEmployee);

            var foundEmployee = await dbStorage.GetAsync(Guid.Parse("33fe71a9-06f1-4028-950b-0469c734638b"));
            var newDateEmployee = new Employee(foundEmployee.Id, foundEmployee.FullName, foundEmployee.Birthday, foundEmployee.ContractEmployee, "0001 010101");

            //Act
            bool result = await dbStorage.UpdateAsync(Guid.Parse("33fe71a9-06f1-4028-950b-0469c734638b"), newDateEmployee);
            var updatedEmployee = await dbStorage.GetAsync(Guid.Parse("33fe71a9-06f1-4028-950b-0469c734638b"));

            //Assert
            Assert.True(result);
            Assert.Equal("0001 010101", updatedEmployee.PassportNumber);
            Assert.NotEqual("1612 633276", updatedEmployee.PassportNumber);
            Assert.Equal(foundEmployee.FullName, updatedEmployee.FullName);
        }

        [Fact]
        public async Task Delete_test()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using BankSystemDbContext dbContext = new BankSystemDbContext(options);
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);

            Guid testId = Guid.NewGuid();
            Employee deletedEmployye = new Employee(testId, "Тестоый сотрудник 007", new DateTime(2000, 1, 1),
                new EmployeeContract(new DateTime(2025, 7, 18), null, 3000, "Backend-developer"), "0020 020202");

            //Act
            var resultAdd = await dbStorage.AddAsync(deletedEmployye);
            var foundEmployee = await dbStorage.GetAsync(deletedEmployye.Id);
            var resultDelete = await dbStorage.DeleteAsync(deletedEmployye.Id);

            //Assert
            Assert.True(resultAdd);
            Assert.True(resultDelete);
            Assert.NotNull(foundEmployee);
        }

        [Fact]
        public async Task UpdateContract_Test()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb_UpdateContract")
                .Options;

            using BankSystemDbContext dbContext = new BankSystemDbContext(options);
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);

            var testContract = new EmployeeContract(new DateTime(2024, 6, 6), null, 2500, "Junior Backend Developer");
            var testEmployee = new Employee(Guid.Parse("3af9105a-b9e1-497a-aa3d-c237aef22ba9"), "Тестовый сотрудник", new DateTime(2000, 2, 2),
                testContract, "4354 543657");

            await dbStorage.AddAsync(testEmployee);
            EmployeeContract newDataContract = new EmployeeContract(DateTime.Now, null, 3600, "Middle Backend Developer");


            //Act
            //bool resultFirstData = await dbStorage.UpdateContractAsync(Guid.Parse("3af9105a-b9e1-497a-aa3d-c237aef22ba9"), testContract);
            bool result = await dbStorage.UpdateContractAsync(Guid.Parse("3af9105a-b9e1-497a-aa3d-c237aef22ba9"), newDataContract);
            var foundEmployee = await dbStorage.GetAsync(Guid.Parse("3af9105a-b9e1-497a-aa3d-c237aef22ba9"));

            //Assert
            //Assert.True(resultFirstData);
            Assert.True(result);
            Assert.Equal("Middle Backend Developer", foundEmployee.ContractEmployee.Post);
            Assert.NotEqual(2500, foundEmployee.ContractEmployee.Salary);
        }

        [Fact]
        public async Task CreateClientProfileAndAccount_Test()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using BankSystemDbContext dbContext = new BankSystemDbContext(options);
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);
            TestDataGenerator generator = new TestDataGenerator();

            var testData_EmailAndPhone = generator.GenerateTestListClients(1).First();

            var EmployeeTest = generator.GenerateTestListEmployee(1).First();
            Currency testCurrency = new Currency("GBP", '£');

            //Act
            bool resultAdd = await dbStorage.AddAsync(EmployeeTest);

            bool result = await dbStorage.CreateClientProfileAndAccountAsync(EmployeeTest.Id, testCurrency,
                testData_EmailAndPhone.Email, testData_EmailAndPhone.PhoneNumber);

            var foundEmployee = await dbStorage.GetAsync(EmployeeTest.Id);

            //Assert
            Assert.True(result);
            Assert.True(resultAdd);
            Assert.NotNull(foundEmployee);
            Assert.NotNull(foundEmployee.ClientProfile);
            Assert.Equal("GBP", foundEmployee.ClientProfile.Accounts.First().CurrencyCode);

        }

        [Fact]
        public async Task DeleteAccount_Test()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using BankSystemDbContext dbContext = new BankSystemDbContext(options);
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);

            var testContract = new EmployeeContract(new DateTime(2024, 6, 6), null, 2500, "Junior Backend Developer");
            var testEmployee = new Employee(Guid.Parse("141a712b-47bb-4a61-9379-8561605da2d6"), "Тестовый сотрудник", new DateTime(2000, 2, 2),
                testContract, "4354 543657");

            await dbStorage.AddAsync(testEmployee);


            var currency = new Currency("EUR", '€');

            bool resultAdd = await dbStorage.CreateClientProfileAndAccountAsync(Guid.Parse("141a712b-47bb-4a61-9379-8561605da2d6"),
                currency, "test6@mail.ru", "+794854366");

            var foundEmployee = await dbStorage.GetAsync(Guid.Parse("141a712b-47bb-4a61-9379-8561605da2d6"));
            var foundClientProfile = foundEmployee.ClientProfile;

            //Act
            var foundAccount = foundClientProfile.Accounts.FirstOrDefault(c => c.CurrencyCode == "EUR");

            bool resultDelete = await dbStorage.DeleteAccountAsync(Guid.Parse("141a712b-47bb-4a61-9379-8561605da2d6"), foundAccount.Id);

            //Assert
            Assert.NotNull(foundAccount);
            Assert.True(resultAdd);
            Assert.True(resultDelete);
        }

        [Fact]
        public async Task UpdateAccount_Test()
        {

            // Arrange
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb_UpdateAccount")
                .Options;

            using var dbContext = new BankSystemDbContext(options);
            var dbStorage = new EmployeeDbStorage(dbContext);

            var testContract = new EmployeeContract(
                new DateTime(2024, 6, 6), null, 2500, "Junior Backend Developer");

            var testEmployee = new Employee(
                Guid.Parse("717f5ba7-5fdb-484d-a948-319143a61ecd"),
                "Тестовый сотрудник",
                new DateTime(2000, 2, 2),
                testContract,
                "4354 543657");

            await dbStorage.AddAsync(testEmployee);

            bool resultAdd = await dbStorage.CreateClientProfileAndAccountAsync(
                testEmployee.Id,
                new Currency("EUR", '€'),
                "test3@mail.ru",
                "4354 546578");

            var foundEmployee = await dbStorage.GetAsync(testEmployee.Id);

            var foundOldAccount = foundEmployee.ClientProfile.Accounts
                .FirstOrDefault(c => c.CurrencyCode == "EUR");

            var newDataAccount = new Account(
                foundEmployee.ClientProfile.Id,
                new Currency("GBP", '£'),
                999);

            // Act 
            bool resultUpdate = await dbStorage.UpdateAccountAsync(
                foundEmployee.Id,
                foundOldAccount.Id,
                newDataAccount);

            // Assert
            Assert.True(resultAdd);
            Assert.NotNull(foundOldAccount);
            Assert.True(resultUpdate);

            var updatedEmployee = await dbStorage.GetAsync(testEmployee.Id);
            var updatedAccount = updatedEmployee.ClientProfile.Accounts
                .FirstOrDefault(c => c.Id == foundOldAccount.Id);

            Assert.Equal("GBP", updatedAccount.CurrencyCode);
            Assert.Equal(999, updatedAccount.Amount);
        }

        [Fact]
        public async Task GetFilterEmployees_Test()
        {
            //Arrange

            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=dbBankSystem;Username=postgres;Password=Diana123")
                .Options;

            using BankSystemDbContext dbContext = new BankSystemDbContext(options);
            EmployeeDbStorage dbStorage = new EmployeeDbStorage(dbContext);

            EmployeeFilterDTO filter = new EmployeeFilterDTO()
            {
                SalaryFrom = 5000,
                SalaryTo = 11000

            };

            //Act
            var pagedResult = await dbStorage.GetFilterEmployeesAsync(filter, 1, 10);
            var result = pagedResult.Items;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
        }
    }
}
