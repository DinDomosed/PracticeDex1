using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        public EmployeeContract ContractEmployee {  get; private set; } = null!;
        public string PassportNumber { get; private set; }

        // For EF
        public Client? ClientProfile { get; private set; } = null!;
        public Guid? ClientId { get; private set; }
        protected Employee() : base() { }
        
        public Employee (string FullName, DateTime Birthday, EmployeeContract contract, string passportNumber) : base (FullName, Birthday)
        {
            ContractEmployee = contract;
            PassportNumber = passportNumber;

        }
        public Employee(Guid Id, string FullName, DateTime Birthday, EmployeeContract contract, string passportNumber) : base(Id, FullName, Birthday)
        {
            ContractEmployee = contract;
            PassportNumber = passportNumber;
        }

        public void CreateClientProfile(string email, string phoneNumber)
        {
            ClientProfile = new Client(FullName, Birthday, email, phoneNumber, PassportNumber);
        }

        public void SetContract (EmployeeContract contract)
        {
            ContractEmployee = contract;
        }
        public override string ToString()
        {
            return $"Сотрудник: {FullName}\n" +
                $"Дата Рождения: {Birthday:d}\n" +
                $"Возраст: {Age}\n" +
                $"Информация о контракте:\n\n" +
                $"{ContractEmployee.ToString()}\n" +
                $"${PassportNumber}\n\n" +
                $"ID сотрудника: {Id}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Employee employee))
                return false;

            return FullName == employee.FullName &&
                Birthday == employee.Birthday &&
                ContractEmployee.Post == employee.ContractEmployee.Post &&
                Id == employee.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(FullName, Birthday, ContractEmployee.Post);
        }
    }
}
