using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Exceptions
{
    public class PassportNumberNullOrWhiteSpaceException : ArgumentException
    {
        public PassportNumberNullOrWhiteSpaceException() : base()
        { }

        public PassportNumberNullOrWhiteSpaceException(string message) : base(message)
        { }
        public PassportNumberNullOrWhiteSpaceException(string? message, Exception? innerException) : base(message, innerException)
        { }
        public PassportNumberNullOrWhiteSpaceException(string? messege, string? paramName, Exception? innerException) : base(messege, paramName, innerException) { }
    }
}
