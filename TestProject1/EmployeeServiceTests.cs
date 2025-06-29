using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Exceptions;
using BankSystem.App.Services;
using BankSystem.Data.Storages;
using BankSystem.Domain.Models;

namespace BankSystem.App.Tests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void AddEmployeeTest_Count_9()
        {
            //Arrange

            EmployeeStorage employeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(employeeStorage);

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
            bool result = false;
            foreach (var employee in employees)
            {
                result = employeeService.AddEmployee(employee);
            }

            //Act + Assert
            Assert.Throws<InvalidEmployeeAgeException>(() =>
            {
                employeeService.AddEmployee(new Employee("Тестовый сотрудник9", new DateTime(2009, 9, 6),
                    new EmployeeContract(new DateTime(2020, 6, 24), new DateTime(2060, 6, 24), 1500, "Бекенд разработчик"), "4324 999999"));
            });

            //Assert
            Assert.True(result);
            Assert.Equal(9, employeeStorage.AllEmployeeBank.Count);
        }
        [Fact]
        public void EditingEmployeeContract ()
        {
            //Arrange
            EmployeeStorage employeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(employeeStorage);
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
                employeeService.AddEmployee(employee);
            }

            //Act
            employeeService.EditingEmployeeContract(guidTest, "Тестовый сотрудник1", "4324 111111", newDateEndWork: new DateTime(3000, 1, 1), newSalary: 3500);

            //Assert
            Assert.Equal(new DateTime(3000, 1, 1), employeeStorage.AllEmployeeBank[guidTest].ContractEmployee.EndOfContract);
            Assert.Equal(3500, employeeStorage.AllEmployeeBank[guidTest].ContractEmployee.Salary);
            Assert.Equal("Тестовый сотрудник1", employeeStorage.AllEmployeeBank[guidTest].FullName);
        }

        [Fact]
        public void GetFilterEmployee_Count_4_5()
        {
            //Arrange
            EmployeeStorage employeeStorage = new EmployeeStorage();
            EmployeeService employeeService = new EmployeeService(employeeStorage);

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
                employeeService.AddEmployee(employee);
            }

            //Act
            List<Employee> filterEmployee = employeeService.GetFilterEmployee( fromThisSalary: 3000, beforeThisSalary: 7000);
            var filterEmployee2 = employeeService.GetFilterEmployee(fromThisDateBirthday: new DateTime(2000, 1, 1), beforeThisDateBirthday: new DateTime(2007, 1, 1));
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
    }
}
