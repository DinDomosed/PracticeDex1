using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public struct Currency
    {
        public string Code { get; private set; }
        public char Symbol { get; private set; }

        public Currency(string code, char symbol)
        {
            Code = code;
            Symbol = symbol;
        }
        public override string ToString()
        {
            return $"{Code} {Symbol}";
        }
    }
}
