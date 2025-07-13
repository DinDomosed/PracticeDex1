using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs
{
    public class ClientFilterDTO
    {
        public string? FullName { get; set; }
        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }
        public decimal? Bonus { get; set; }
        public int? CountAccounts { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set;  }
        public string? PassportNumber { get; set; }
        public DateTime? RegisterDateFrom { get; set; }
        public DateTime? RegisterDateTo { get; set; }
    }
}
