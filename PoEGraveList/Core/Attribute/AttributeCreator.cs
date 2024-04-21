using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoEGraveList.Core.Misc;
using PoEGraveList.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoEGraveList.Core.Attribute
{
    public class AttributeCreator
    {
        private static AttributeCreator _instance = null!;
        public static AttributeCreator Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new AttributeCreator();
                return _instance;
            }
        }

        private List<GameAttribute> attributeConfig;
        private AttributeCreator()
        {
            this.attributeConfig = new List<GameAttribute>();
            this.initAttributeList();
        }
        public GameAttribute FromKey(string key)
        {
            return this.attributeConfig.First((attribute) => attribute.Key == key);
        }

        private void initAttributeList()
        {
            GameAttribute[]? attributes = JsonConvert.DeserializeObject<GameAttribute[]>(File.ReadAllText("./itemConfig.json"));
            if (attributes == null) throw new Exception("Can't read attributes from file.");

            this.attributeConfig.AddRange(attributes);
        }
      
    }
}
