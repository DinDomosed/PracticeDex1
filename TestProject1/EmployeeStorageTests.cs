using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class EmployeeStorageTests
    {
        [Fact]
        public void AddEmployeeToStorageTest_Count_10()
        {
            //Arrange
            EmployeeStorage employeeStorage = new EmployeeStorage();
            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6), //18
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),//19
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),//25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5), //20
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),//26
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),//35
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11), //24
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6), //44
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6), //25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),
            };

            //Act
            foreach (var employee in employees)
            {
                employeeStorage.AddEmployeeToStorage(employee);
            }

            //Assert
            Assert.Equal(9, employeeStorage.AllEmployeeBank.Count);
        }

        [Fact]
        public void GetYoungestEmployeeFromStorage()
        {
            //Arrange
            EmployeeStorage employeeStorage = new EmployeeStorage();
            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6), //18
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),//19
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),//25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5), //20
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),//26
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),//35
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11), //24
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6), //44
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6), //25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),
            };

            //Act
            foreach (var employee in employees)
            {
                employeeStorage.AddEmployeeToStorage(employee);
            }
            var youngestEmployee = employeeStorage.AllEmployeeBank.OrderBy(u => u.Value.Birthday).LastOrDefault();

            //Assert
            Assert.NotNull(youngestEmployee);
            Assert.Equal(new DateTime(2006, 9, 6), youngestEmployee.Value.Birthday);
            Assert.Equal("Тестовый сотрудник1", youngestEmployee.Value.FullName);
        }
        [Fact]
        public void GetOlderEmployeeFromStorage ()
        {
            //Arrange 
            EmployeeStorage employeeStorage = new EmployeeStorage();
            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6), //18
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),//19
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),//25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5), //20
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),//26
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),//35
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11), //24
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6), //44
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6), //25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),
            };

            //Act
            foreach (var employee in employees)
            {
                employeeStorage.AddEmployeeToStorage(employee);
            }
            var olderEmployee = employeeStorage.AllEmployeeBank.OrderBy(u => u.Value.Birthday).First().Value;

            //Assert
            Assert.NotNull(olderEmployee);
            Assert.Equal(new DateTime(1980, 9, 6), olderEmployee.Birthday);
            Assert.Equal("Тестовый сотрудник8", olderEmployee.FullName);
        }
        [Fact]
        public void GetAverageAgeEmployee()
        {
            //Arrange
            EmployeeStorage employeeStorage = new EmployeeStorage();
            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6), //18
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),//19
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),//25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5), //20
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),//26
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),//35
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11), //24
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6), //44
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6), //25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик")),
            };

            //Act
            foreach (var employee in employees)
            {
                employeeStorage.AddEmployeeToStorage(employee);
            }
            int sumAge = employeeStorage.AllEmployeeBank.Sum(u => u.Value.Age);
            int result = sumAge / employeeStorage.AllEmployeeBank.Count();

            //Assert
            Assert.Equal(26, result);
        }
    }
}
