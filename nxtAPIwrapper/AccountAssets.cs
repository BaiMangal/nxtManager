using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nxtAPIwrapper
{
    public class AssetBalance
    {
        public int balance { get; set; }
        public string asset { get; set; }
    }

    public class AccountAssets
    {
        public string publicKey { get; set; }
        public int balance { get; set; }
        public List<AssetBalance> assetBalances { get; set; }
        public int effectiveBalance { get; set; }
    }
}
