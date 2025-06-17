using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Employee : Person
    {
        public string ContractEmployee {  get; private set; }

        public Employee (string FullName, DateTime Birthday, EmployeeContract contract) : base (FullName, Birthday)
        {
            ContractEmployee = contract.ToString ();
        }

        public void SetContract (EmployeeContract contract)
        {
            ContractEmployee = contract.ToString ();
        }
        public override string ToString()
        {
            return $"Сотрудник: {FullName}\n" +
                $"Дата Рождения: {Birthday:d}\n" +
                $"Возраст: {Age}\n" +
                $"Информация о контракте:\n\n" +
                $"{ContractEmployee}\n\n" +
                $"ID сотрудника: {Id}";
        }
    }
}
