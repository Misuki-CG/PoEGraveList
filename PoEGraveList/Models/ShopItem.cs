using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoEGraveList.Models
{
    public class ShopItem
    {
        public required GameAttribute Attribute { get; set; }
        public int Amount { get; set; }
        public bool IsUpValue { get; set; }
    }
}
