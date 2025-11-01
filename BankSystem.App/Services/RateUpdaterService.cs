using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;


namespace BankSystem.App.Services
{
    public class RateUpdaterService : BackgroundService
    {
        private readonly IRateProvider _rateProvider;
        private readonly IServiceProvider _serviceProvader;

        public RateUpdaterService(IServiceProvider serviceProvader, IRateProvider rateProvider)
        {
            _serviceProvader = serviceProvader;
            _rateProvider = rateProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan delay = GetDelayUntilNextMonth();

                await Task.Delay(delay, stoppingToken);

                await using var scope = _serviceProvader.CreateAsyncScope();
                var clientStorage = scope.ServiceProvider.GetRequiredService<IClientStorage>();

                await RateUpdaterAsync(clientStorage ,stoppingToken);

            }
        }
        internal async Task RateUpdaterAsync(IClientStorage clientStorage, CancellationToken cancellationToken)
        {
            List<Client> allclients = await clientStorage.GetAllAsync();
            decimal _rate = _rateProvider.GetRate();

            foreach (var client in allclients)
            {
                cancellationToken.ThrowIfCancellationRequested();

                foreach (var account in client.Accounts.ToList())
                {
                    int maxPercentage = 100;
                    decimal accrualAmount = (account.Amount * _rate) / maxPercentage;

                    account.PutMoney(accrualAmount);

                    await clientStorage.UpdateAccountAsync(client.Id, account.AccountNumber, account);
                }
            }
        }
        private TimeSpan GetDelayUntilNextMonth()
        {
            DateTime now = DateTime.Now;
            DateTime nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
            TimeSpan delay = nextMonth - now;
            return delay;
        }
    }

}
