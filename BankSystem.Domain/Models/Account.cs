﻿using System;
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
        public decimal Amount { get; private set; }
        public string AccountNumber { get; private set; }
        public Guid Id { get; private set; }

        //For EF
        public Currency Currency { get; private set; } = null!;
        public string CurrencyCode { get; private set; }
        public Guid IdClient { get; private set; }
        public Client Client { get; private set; }
        protected Account() { }


        public Account (Guid idClient, Currency currency, decimal amount, string? accountNumber = null, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            Currency = currency;
            Amount = amount;
            AccountNumber = accountNumber ?? GenerateAccountNumber();
            IdClient = idClient;
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
            CurrencyCode = currency.Code;
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
