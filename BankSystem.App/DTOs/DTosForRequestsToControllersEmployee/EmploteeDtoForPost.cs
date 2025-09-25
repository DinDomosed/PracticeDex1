using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs.DTosForRequestsToControllersEmployee
{
    public class EmploteeDtoForPost
    {
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string PassportNumber { get; set; }
        public EmployeeContractDtoForPost ContractEmployee { get; set; }
    }
}
