using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs.DTOsAccounts
{
    public class AccountDTOForPost
    {
        public Guid IdClient { get; set; }
        public decimal? Amount { get; set; }
        public CurrencyDtoForPost Currency { get; set; }

    }
}
