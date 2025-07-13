using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Interfaces
{
    public interface IStorage <T>
    {
        public T? Get(Guid Id);
        public List<T>? GetAll();
        public bool Add(T item);
        public bool Update(Guid Id,T item);
        public bool Delete(Guid Id);

        public bool Exists(Guid id, string passoprtNumber);
    }
}
