using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace BankSystem.App.Exceptions
{
    public class InvalidClientAgeException : ArgumentException
    {
        public InvalidClientAgeException() : base()
        {

        }
        public InvalidClientAgeException(string message) : base(message)
        {

        }
        //inner для сохранения исходного исключения
        public InvalidClientAgeException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
        //paramName для сохранения параметра из за которого произошло исключение
        public InvalidClientAgeException(string? message, string? paramName, Exception? innerException) : base(message, paramName, innerException)
        {

        }
    }
}
