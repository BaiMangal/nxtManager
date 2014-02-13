using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nxtAPIwrapper
{
    public class StartForgingResult
    {
        public static DateTime GenesisBlockTime = new DateTime(2013, 11, 24, 12, 0, 0, 0);
        public string deadline { get; set; }
        public DateTime Date
        {
            get
            {
                if (deadline == null)
                    deadline = "0";
                return GenesisBlockTime.AddSeconds(Double.Parse(deadline)).Add(DateTime.Now.Subtract(DateTime.UtcNow));
            }
        }
    }
}
