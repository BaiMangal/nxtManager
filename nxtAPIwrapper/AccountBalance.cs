using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nxtAPIwrapper
{
    public class AccountBalance
    {
        public double balance { get; set; }
        public double effectiveBalance { get; set; }
        public double unconfirmedBalance { get; set; }

        public double formattedBalance { get { return balance / 100.0; } }
        public string formattedUnconfirmedBalance { get { return (unconfirmedBalance / 100.0).ToString("#,##0"); } }
        public double formattedEffectiveBalance { get { return effectiveBalance / 100.0; } }

    }
}