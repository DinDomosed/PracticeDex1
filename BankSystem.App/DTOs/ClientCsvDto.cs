using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs
{
    public class ClientCsvDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public decimal? Bonus { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportNumber { get; set; }
        public DateTime RegistrationDate { get; set; }

    }
}
