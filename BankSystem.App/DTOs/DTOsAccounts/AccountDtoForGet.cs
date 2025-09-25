using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs.DTOsAccounts
{
    public class AccountDtoForGet
    {
        public Guid Id { get;  set; }
        public decimal Amount { get;  set; }
        public string AccountNumber { get;  set; }
        public string CurrencyCode { get;  set; }
    }
}
