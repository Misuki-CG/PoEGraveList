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

        internal void SetAttributeDescription(bool isUpValue)
        {
            this.Description = isUpValue ?
                $"{ValueUp}% increased chance of {Name} modifiers" :
                $"{Name} modifiers are {ValueDown}% scarcer";
        }

        internal void SetAttributeDescription(string description)
        {
            this.Description = description;
        }
    }
}
