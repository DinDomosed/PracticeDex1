using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs.DTOsAccounts
{
    public class CurrencyDtoForPut
    {
        public string Code { get; set; }
        public char? Symbol { get; set; }
    }
}
