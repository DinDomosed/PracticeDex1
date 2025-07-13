using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.DTOs
{
    public class EmployeeFilterDTO
    {
        public string? FullName { get; set; }
        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }
        public decimal? Bonus { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? StartWorkDateFrom { get; set; }
        public DateTime? EndContractDateTo { get; set; }
        public decimal? SalaryFrom { get; set; }
        public decimal? SalaryTo { get; set; }
        public string? Post { get; set; }
    }
}
