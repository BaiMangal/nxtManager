using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nxtAPIwrapper
{
    public class State
    {
        public string lastBlock { get; set; }
        public string numberOfBlocks { get; set; }
        public string lastBlockchainFeeder { get; set; }
        public string totalMemory { get; set; }
        public bool isCatchingUp { get; set; }
        public string freeMemory { get; set; }
        public string maxMemory { get; set; }
        public string numberOfTransactions { get; set; }
        public string numberOfUsers { get; set; }
        public string version { get; set; }
        public string numberOfOrders { get; set; }
        public string time { get; set; }
        public string availableProcessors { get; set; }
        public string numberOfAssets { get; set; }
        public string numberOfAccounts { get; set; }
    }
}
