using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nxtAPIwrapper
{
    public class Transaction
    {
        public static DateTime GenesisBlockTime = new DateTime(2013, 11, 24, 12, 0, 0, 0);

        public string block { get; set; }
        public string timestamp { get; set; }
        public string deadline { get; set; }
        public string sender { get; set; }
        public string recipient { get; set; }
        public string amount { get; set; }
        public string fee { get; set; }
        public string confirmations { get; set; }
        public string signature { get; set; }

        public DateTime Date
        {
            get
            {
                if (timestamp == null)
                    timestamp = "0";
                return GenesisBlockTime.AddSeconds(Double.Parse(timestamp)).Add(DateTime.Now.Subtract(DateTime.UtcNow));
            }
        }
        public string formattedConfirmations { get { return (confirmations != null && (Double.Parse(confirmations) > 10) ? "10+" : confirmations); } }
        public bool IsSenderEqualToRecipient { get { return sender == recipient; } }
    }
}
