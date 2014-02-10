using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nxtAPIwrapper
{
    public class Block
    {
        public static DateTime GenesisBlockTime = new DateTime(2013, 10, 24, 12, 0, 0, 0);

        public string blockID { get; set; }
        public List<string> transactions { get; set; }
        public string nextBlock { get; set; }
        public string blockSignature { get; set; }
        public double payloadLength { get; set; }
        public int numberOfTransactions { get; set; }
        public string version { get; set; }
        public string timestamp { get; set; }
        public string previousBlock { get; set; }
        public string payloadHash { get; set; }
        public string height { get; set; }
        public string totalFee { get; set; }
        public string baseTarget { get; set; }
        public string generationSignature { get; set; }
        public string previousBlockHash { get; set; }
        public string totalAmount { get; set; }
        public string generator { get; set; }

        public string totalAmountAndFee { get { return totalAmount + " + " + totalFee; } }

        public DateTime Date { get { return GenesisBlockTime.AddSeconds(Double.Parse(timestamp)).Add(DateTime.Now.Subtract(DateTime.UtcNow)); } }
    }
}