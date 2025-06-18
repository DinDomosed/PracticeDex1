using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Account : INotifyPropertyChanged
    {
        public Currency Currency { get; private set; }
        public decimal Amount { get; private set; }

        public Account (Currency currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged (string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs (propertyName));
        }

        public void PutMoney(decimal sum)
        {
            Amount += Math.Abs(sum);
            OnPropertyChanged(nameof(Amount));
            
        }
        public decimal GetMoney(decimal sum)
        {
            if (Amount >= sum)
            {
                Amount -= Math.Abs(sum);
                OnPropertyChanged (nameof(Amount));
            }
            return Amount;

        }

    }
}
