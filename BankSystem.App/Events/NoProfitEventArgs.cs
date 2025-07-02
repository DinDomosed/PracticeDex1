using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Events
{
    public class NoProfitEventArgs : EventArgs
    {
        public decimal NetProfit { get; private set; }

        public NoProfitEventArgs(decimal netProfit)
        {
            NetProfit = netProfit;
        }
    }
}
