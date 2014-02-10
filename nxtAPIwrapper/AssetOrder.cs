using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nxtAPIwrapper
{
    public class AssetOrder
    {
     public string account {get;set;} 
     public string asset {get;set;}
     public int quantity {get;set;}
     public int price { get; set; }
    }
}
