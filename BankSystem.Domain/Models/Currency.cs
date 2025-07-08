using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Currency
    {
        public string Code { get; private set; }
        public char Symbol { get; private set; }

        // For EF
        public List<Account> Accounts { get; private set; } = null!;

        public Currency(string code, char symbol)
        {
            Code = code;
            Symbol = symbol;
        }

        public override string ToString()
        {
            return $"{Code} {Symbol}";
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj is not Currency curr)
                return false;
            return Code == curr.Code && Symbol == curr.Symbol;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Symbol);
        }
    }
}
