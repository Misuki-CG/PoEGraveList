using PoEGraveList.Models.Types;
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
        public string Name { get; set; }
        public GameAttributeType Type { get; set; }

        public GameAttribute(GameAttributeType type)
        {
            Type = type;
            Name = Enum.GetName(type) ?? "Undefined";
        }



    }
}
