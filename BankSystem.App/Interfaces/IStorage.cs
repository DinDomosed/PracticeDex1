using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IStorage <T>
    {
        public List<T> Get(Func<T, bool>? predicate = null);
        public bool Add(T item);
        public bool Update(T item);
        public bool Delete(T item);
    }
}
