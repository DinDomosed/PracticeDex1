using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;

namespace BankSystem.App.Events
{
    public class OwnersSalariesEventArgs : EventArgs
    {
        public Employee Owner { get; private set; }
        public decimal Salary { get; private set; }

        public OwnersSalariesEventArgs(Employee owner, decimal salary)
        {
            Owner = owner;
            Salary = salary;
        }
    }
}
