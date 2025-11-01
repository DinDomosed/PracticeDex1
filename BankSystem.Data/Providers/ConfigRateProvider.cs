using BankSystem.App.Interfaces;
using BankSystem.App.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.Providers
{
    public class ConfigRateProvider : IRateProvider
    {
        private readonly decimal _rate;
        public ConfigRateProvider(IOptions<RateOptions> options)
        {
            _rate = options.Value.Rate;
        }
        public decimal GetRate()
        {
            return _rate;
        }
    }
}
