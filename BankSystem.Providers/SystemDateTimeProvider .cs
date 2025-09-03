using BankSystem.App.Interfaces;

namespace BankSystem.Providers
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
