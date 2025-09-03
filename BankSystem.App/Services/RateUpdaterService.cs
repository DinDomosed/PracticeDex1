﻿using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;


namespace BankSystem.App.Services
{
    public class RateUpdaterService : BackgroundService
    {
        private readonly decimal _rate;
        private IClientStorage _clientStorage;
        private IDateTimeProvider _dateProvider;
        public RateUpdaterService(IClientStorage storage, decimal rate, IDateTimeProvider dateTimeProvider)
        {
            _clientStorage = storage;
            _rate = rate;
            _dateProvider = dateTimeProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan delay = GetDelayUntilNextMonth();

                await Task.Delay(delay, stoppingToken);

                await RateUpdaterAsync(stoppingToken);

            }
        }
        internal async Task RateUpdaterAsync(CancellationToken cancellationToken)
        {
            List<Client> allclients = await _clientStorage.GetAllAsync();

            foreach (var client in allclients)
            {
                cancellationToken.ThrowIfCancellationRequested();

                foreach (var account in client.Accounts.ToList())
                {
                    int maxPercentage = 100;
                    decimal accrualAmount = (account.Amount * _rate) / maxPercentage;

                    account.PutMoney(accrualAmount);

                    await _clientStorage.UpdateAccountAsync(client.Id, account.AccountNumber, account);
                }
            }
        }
        private TimeSpan GetDelayUntilNextMonth()
        {
            DateTime now = _dateProvider.Now;
            DateTime nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
            TimeSpan delay = nextMonth - now;
            return delay;
        }
    }

}
