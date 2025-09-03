using BankSystem.App.DTOs;
using BankSystem.App.Exceptions;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public async Task AddEmployee_Test_Count_9()
        {
            //Arrange

            IEmployeeStorage EmployeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(EmployeeStorage);

            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111"),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 222222"),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 333333"),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 444444"),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 555555"),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 666666"),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 777777"),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 888888"),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"),
            };

            //Act
            bool result = false;
            foreach (var employee in employees)
            {
                result = await employeeService.AddEmployeeAsync(employee);
            }

            //Act + Assert
            await Assert.ThrowsAsync<InvalidEmployeeAgeException>(async () =>
            {
                await employeeService.AddEmployeeAsync(new Employee("Тестовый сотрудник9", new DateTime(2009, 9, 6),
                    new EmployeeContract(new DateTime(2020, 6, 24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"));
            });

            var allEmployee = await EmployeeStorage.GetAllAsync();

            //Assert
            Assert.True(result);
            Assert.Equal(9, allEmployee.Count);
        }

        [Fact]
        public async Task UpdateEmployee_Test()
        {
            //Arrange 
            IEmployeeStorage EmployeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(EmployeeStorage);

            Guid TestId = Guid.NewGuid();
            List<Employee> employees = new List<Employee>()
            {
                new Employee(TestId,"Тестовый сотрудник1", new DateTime(2006, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111"),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 222222"),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 333333"),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 444444"),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 555555"),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 666666"),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 777777"),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 888888"),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"),
            };

            foreach (var emp in employees)
            {
                await employeeService.AddEmployeeAsync(emp);
            }
            Employee employeeTest = await EmployeeStorage.GetAsync(TestId);


            //Act 
            Employee newDataEmployee = new Employee(TestId, "Тестовый сотрудник1000", new DateTime(2000, 9, 6),
                new EmployeeContract(new DateTime(2025, 7, 1), new DateTime(2070, 7, 1), 5000, "Бекенд разработчик"), "4324 111111");

            await employeeService.UpdateEmployeeAsync(newDataEmployee.Id, newDataEmployee);

            //Assert
            Assert.NotEqual(employeeTest.FullName, newDataEmployee.FullName);

        }

        [Fact]
        public async Task DeleteEmployee_Test()
        {
            //Arrange 
            IEmployeeStorage employeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(employeeStorage);

            Guid TestId = Guid.NewGuid();
            List<Employee> employees = new List<Employee>()//9
            {
                new Employee(TestId,"Тестовый сотрудник1", new DateTime(2006, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111"),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 222222"),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 333333"),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 444444"),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 555555"),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 666666"),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 777777"),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 888888"),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"),
            };

            foreach (var emp in employees)
            {
                await employeeService.AddEmployeeAsync(emp);
            }

            Employee deleteEmployee = await employeeStorage.GetAsync(TestId);


            //Act
            await employeeService.DeleteEmployeeAsync(deleteEmployee.Id);
            var allEmployees = await employeeStorage.GetAllAsync();
            bool result = allEmployees.Any(u => u.Id == TestId);

            //Assert
            Assert.Equal(8, allEmployees.Count);
            Assert.False(result);
        }
        [Fact]
        public async Task UpdateEmployeeContract_Test_Equal_True()
        {
            //Arrange
            IEmployeeStorage EmployeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(EmployeeStorage);
            Guid guidTest = Guid.NewGuid();

            List<Employee> employees = new List<Employee>()
            {
                new Employee(guidTest, "Тестовый сотрудник1", new DateTime(2006, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111"),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 222222"),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 333333"),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 444444"),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 555555"),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 666666"),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 777777"),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 888888"),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"),
            };

            foreach (var employee in employees)
            {
                await employeeService.AddEmployeeAsync(employee);
            }
            EmployeeContract newContract = new EmployeeContract(new DateTime(2020, 6, 24), new DateTime(3000, 1, 1), 3500, "Бекенд разработчик");

            //Act
            await employeeService.UpdateEmployeeContractAsync(guidTest, newContract);

            var foundEmployee = await EmployeeStorage.GetAsync(guidTest);

            //Assert
            Assert.Equal(new DateTime(3000, 1, 1), foundEmployee.ContractEmployee.EndOfContract);
            Assert.Equal(3500, foundEmployee.ContractEmployee.Salary);
            Assert.Equal("Тестовый сотрудник1", foundEmployee.FullName);
        }

        [Fact]
        public async Task GetFilterEmployee_Test_Count_4_5()
        {
            //Arrange
            IEmployeeStorage EmployeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(EmployeeStorage);

            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111"),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 2500, "Бекенд разработчик"), "4324 222222"),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 3500, "Бекенд разработчик"), "4324 333333"),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 4500, "Бекенд разработчик"), "4324 444444"),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 5500, "Бекенд разработчик"), "4324 555555"),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 6500, "Бекенд разработчик"), "4324 666666"),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 7500, "Бекенд разработчик"), "4324 777777"),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 8500, "Бекенд разработчик"), "4324 888888"),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6),
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 9500, "Бекенд разработчик"), "4324 999999"),
            };

            foreach (var employee in employees)
            {
                await employeeService.AddEmployeeAsync(employee);
            }


            //Act
            EmployeeFilterDTO filter1 = new EmployeeFilterDTO
            {
                SalaryFrom = 3000,
                SalaryTo = 7000
            };

            EmployeeFilterDTO filter2 = new EmployeeFilterDTO
            {
                BirthDateFrom = new DateTime(2000, 1, 1),
                BirthDateTo = new DateTime(2007, 1, 1)
            };

            var resultFilter1 = await employeeService.GetFilterEmployeeAsync(filter1, 1, 10);
            var resultfilter2 = await employeeService.GetFilterEmployeeAsync(filter2, 1);

            List<Employee> filterEmployee = resultFilter1.Items;
            var filterEmployee2 = resultfilter2.Items;

            filterEmployee = filterEmployee.OrderBy(u => u.ContractEmployee.Salary).ToList();
            filterEmployee2 = filterEmployee2.OrderBy(u => u.Birthday).ToList();


            //Assert
            Assert.Equal(4, filterEmployee.Count);
            Assert.Equal(3500, filterEmployee[0].ContractEmployee.Salary);
            Assert.Equal(4500, filterEmployee[1].ContractEmployee.Salary);
            Assert.Equal(5500, filterEmployee[2].ContractEmployee.Salary);
            Assert.Equal(6500, filterEmployee[3].ContractEmployee.Salary);

            Assert.Equal(5, filterEmployee2.Count);

            Assert.Equal(new DateTime(2000, 6, 14), filterEmployee2[0].Birthday);
            Assert.Equal(new DateTime(2000, 9, 11), filterEmployee2[1].Birthday);
            Assert.Equal(new DateTime(2004, 9, 5), filterEmployee2[2].Birthday);
            Assert.Equal(new DateTime(2005, 10, 19), filterEmployee2[3].Birthday);
            Assert.Equal(new DateTime(2006, 9, 6), filterEmployee2[4].Birthday);
        }

        [Fact]
        public async Task CreateAccountProfile_Test()
        {
            //Arrange
            IEmployeeStorage employeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(employeeStorage);

            Guid TestId = Guid.NewGuid();
            Employee employeeTest = new Employee(TestId, "Тестовый сотрудник1", new DateTime(2006, 9, 6),
                new EmployeeContract(new DateTime(2020, 6, 24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111");

            var currnecy = new Currency("USD", '$');

            await employeeService.AddEmployeeAsync(employeeTest);


            //Act
            await employeeService.CreateAccountProfileAsync(employeeTest.Id, currnecy, "testEmpCl@mail.ru", "+7 918 111 12 12");

            //Assert
            Assert.NotEqual(null, employeeTest.ClientProfile);
        }
    }
}
