using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs.DTOsAccounts
{
    public class AccountDtoForPut
    {
        public decimal? Amount { get;  set; }
        public CurrencyDtoForPut? Currency { get;  set; } 
    }
}
