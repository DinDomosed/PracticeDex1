using BankSystem.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Tests
{
    public class FakeSystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get; }

        public FakeSystemDateTimeProvider(DateTime? dateTime)
        {
            Now = dateTime ?? DateTime.Now;
        }
    }
}
