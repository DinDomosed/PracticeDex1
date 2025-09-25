using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs.DTosForRequestsToControllersEmployee
{
    public class EmployeeDtoForGet
    {
        public Guid Id { get;  set; }
        public string FullName { get;  set; }
        public DateTime Birthday { get;  set; }
        public int Age { get; set; }
        public decimal? Bonus { get;  set; }
        public string PassportNumber { get;  set; }
        public bool HasAClientProfile { get; set; }
        public EmployeeContractDtoForGet ContractEmployee { get;  set; }

    }
}
