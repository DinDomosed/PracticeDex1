using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Exceptions
{
    public class InvalidEmployeeAgeException : ArgumentException
    {
        public InvalidEmployeeAgeException() { }

        public InvalidEmployeeAgeException(string message) : base(message) { }

        public InvalidEmployeeAgeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        public InvalidEmployeeAgeException(string? message, string? paramName) : base(message, paramName)
        {
        }
    }
}
