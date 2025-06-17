using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        public EmployeeContract ContractEmployee {  get; private set; }

        public Employee (string FullName, DateTime Birthday, EmployeeContract contract) : base (FullName, Birthday)
        {
            ContractEmployee = contract;
        }
        public Employee(Guid Id, string FullName, DateTime Birthday, EmployeeContract contract) : base(Id, FullName, Birthday)
        {
            ContractEmployee = contract;
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
                $"{ContractEmployee.ToString()}\n\n" +
                $"ID сотрудника: {Id}";
        }
    }
}
