using BankSystem.App.DTOs.DTOsAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs.DTosForRequestsToControllersEmployee
{
    public class EmployeeClientProfileDtoForPost
    {
        public CurrencyDtoForPost Currency { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
