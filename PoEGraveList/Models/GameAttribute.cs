using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PoEGraveList.Models
{
    public class GameAttribute
    {
   
        public required string Key { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int ValueUp { get; set; }
        public int ValueDown { get; set; }

    }
}
