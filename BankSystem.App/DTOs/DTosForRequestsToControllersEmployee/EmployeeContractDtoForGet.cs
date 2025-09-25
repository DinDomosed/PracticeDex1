using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs.DTosForRequestsToControllersEmployee
{
    public class EmployeeContractDtoForGet
    {
        public Guid Id { get; set; }
        public DateTime StartOfWork { get; set; }
        public DateTime? EndOfContract { get; set; } 
        public decimal Salary { get; set; }
        public string Post { get; set; }
    }
}
