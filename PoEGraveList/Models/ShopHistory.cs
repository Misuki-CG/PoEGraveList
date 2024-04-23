using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoEGraveList.Models
{
    public class ShopHistory
    {
        public required int Id { get; set; }
        public required ShopItem[] ShopItems { get; set; }
        public required string ShopName { get; set; }
        public DateTime QueryDate { get; set; }
    }
}
