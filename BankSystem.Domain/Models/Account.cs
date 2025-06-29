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
        public string AccountNumber { get; private set; }


        public Account (Currency currency, decimal amount, string? accountNumber = null)
        {
            Currency = currency;
            Amount = amount;
            AccountNumber = accountNumber ?? GenerateAccountNumber();
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
        private string GenerateAccountNumber()
        {
            Random random = new Random();
            return "4081" + random.Next(100000000, 999999999).ToString();
        }
        public void EditAccount(Currency currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"Данные счета:\n" +
                $"Номер счета {AccountNumber}" +
                $"Валюта счета: {Currency.Code}\n" +
                $"Сумма на счету: {Amount:C}";
        }

    }
}
