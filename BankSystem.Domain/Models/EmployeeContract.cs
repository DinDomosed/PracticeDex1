using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class EmployeeContract
    {
        public DateTime StartOfWork { get; private set; }
        public DateTime? EndOfContract { get; private set; }
        public decimal Salary { get; private set; }
        public string Post { get; private set; }
        public Guid Id { get; private set; } = Guid.NewGuid();

        // For EF
        public Guid EmployeeId { get; private set; }
        public Employee Employee { get; private set; } = null!;
        protected EmployeeContract() { }

        public EmployeeContract(DateTime startOfWork, DateTime? EndOfContract, decimal Salary, string Post)
        {
            StartOfWork = startOfWork;
            this.EndOfContract = EndOfContract;
            this.Salary = Salary;
            this.Post = Post;
        }
        public void SetSalaryForOwner (decimal salary)
        {
            this.Salary = salary;
        }
        public override string ToString()
        {
            return $"Дата заключения контракта: {StartOfWork:d}\n" +
               $"Дата окончания контракта: {EndOfContract:d}\n" +
               $"Должность: {Post}\n" +
               $"Зарплата: {Salary:C}";
        }
    }
}
