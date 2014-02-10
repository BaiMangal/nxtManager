using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nxtAPIwrapper
{
    public class Alias
    {
        public string errorCode { get; set; }
        public string errorDescription { get; set; }
        public string[] transactionId { get; set; }
        public string alias { get; set; }
        public string uri { get; set; }
    }
}
