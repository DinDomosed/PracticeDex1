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
        public Task<T?> GetAsync(Guid Id);
        public Task<List<T>?> GetAllAsync();
        public Task<bool> AddAsync(T item);
        public Task<bool> UpdateAsync(Guid Id,T item);
        public Task<bool> DeleteAsync(Guid Id);

        public Task<bool> ExistsAsync(Guid id, string passoprtNumber);
    }
}
