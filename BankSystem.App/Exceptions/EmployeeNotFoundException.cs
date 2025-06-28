using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException() : base()
        { }

        public EmployeeNotFoundException(string message) : base(message)
        { }
        public EmployeeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        { }
    }
}
