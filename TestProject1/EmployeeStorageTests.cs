using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.DTOs;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class EmployeeStorageTests
    {
        [Fact]
        public void AddEmployeeToStorageTest_Count_10()
        {
            //Arrange
            IEmployeeStorage fakeEmployeeStorage = new FakeEmployeeStorage();
            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6), //18
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111"),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),//19
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 222222"),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),//25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 333333"),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5), //20
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 444444"),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),//26
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 555555"),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),//35
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 666666"),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11), //24
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 777777"),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6), //44
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 888888"),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6), //25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"),
            };

            //Act
            foreach (var employee in employees)
            {
                fakeEmployeeStorage.Add(employee);
            }
            ClientFilterDTO filter = new ClientFilterDTO();

            //Assert
            Assert.Equal(9, fakeEmployeeStorage.GetAll().Count());
        }

        [Fact]
        public void GetYoungestEmployeeFromStorage()
        {
            //Arrange
            IEmployeeStorage fakeEmployeeStorage = new FakeEmployeeStorage();
            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6), //18
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111"),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),//19
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 222222"),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),//25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 333333"),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5), //20
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 444444"),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),//26
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 555555"),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),//35
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 666666"),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11), //24
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 777777"),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6), //44
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 888888"),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6), //25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"),
            };

            //Act
            foreach (var employee in employees)
            {
                fakeEmployeeStorage.Add(employee);
            }
            var youngestEmployee = fakeEmployeeStorage.GetAll().OrderBy(u => u.Birthday).LastOrDefault();

            //Assert
            Assert.NotNull(youngestEmployee);
            Assert.Equal(new DateTime(2006, 9, 6), youngestEmployee.Birthday);
            Assert.Equal("Тестовый сотрудник1", youngestEmployee.FullName);
        }
        [Fact]
        public void GetOlderEmployeeFromStorage ()
        {
            //Arrange 
            IEmployeeStorage fakeEmployeeStorage = new FakeEmployeeStorage();
            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6), //18
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111"),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),//19
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 222222"),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),//25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 333333"),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5), //20
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 444444"),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),//26
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 555555"),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),//35
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 666666"),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11), //24
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 777777"),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6), //44
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 888888"),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6), //25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"),
            };

            //Act
            foreach (var employee in employees)
            {
                fakeEmployeeStorage.Add(employee);
            }
            var olderEmployee = fakeEmployeeStorage.GetAll().OrderBy(u => u.Birthday).First();

            //Assert
            Assert.NotNull(olderEmployee);
            Assert.Equal(new DateTime(1980, 9, 6), olderEmployee.Birthday);
            Assert.Equal("Тестовый сотрудник8", olderEmployee.FullName);
        }
        [Fact]
        public void GetAverageAgeEmployee()
        {
            //Arrange
            IEmployeeStorage fakeEmployeeStorage = new FakeEmployeeStorage();
            List<Employee> employees = new List<Employee>()
            {
                new Employee("Тестовый сотрудник1", new DateTime(2006, 9, 6), //18
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 111111"),

                new Employee("Тестовый сотрудник2", new DateTime(2005, 10, 19),//19
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 222222"),

                new Employee("Тестовый сотрудник3", new DateTime(2000, 6, 14),//25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 333333"),

                new Employee("Тестовый сотрудник4", new DateTime(2004, 9, 5), //20
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 444444"),

                new Employee("Тестовый сотрудник5", new DateTime(1999, 3, 12),//26
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 555555"),

                new Employee("Тестовый сотрудник6", new DateTime(1990, 4, 30),//35
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 666666"),

                new Employee("Тестовый сотрудник7", new DateTime(2000, 9, 11), //24
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 777777"),

                new Employee("Тестовый сотрудник8", new DateTime(1980, 9, 6), //44
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 888888"),

                new Employee("Тестовый сотрудник9", new DateTime(1999, 9, 6), //25
                new EmployeeContract(new DateTime(2020,6,24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"),
            };

            //Act
            foreach (var employee in employees)
            {
                fakeEmployeeStorage.Add(employee);
            }
            int sumAge = fakeEmployeeStorage.GetAll().Sum(u => u.Age);
            int result = sumAge / fakeEmployeeStorage.GetAll().Count();

            //Assert
            Assert.Equal(26, result);
        }
    }
}
