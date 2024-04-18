using Newtonsoft.Json.Linq;
using PoEGraveList.Models;
using PoEGraveList.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoEGraveList.Core.Attribute
{
    public static class AttributeCreator
    {
        public static GameAttribute FromKey(string attributeKey)
        {
            GameAttributeType attributeType = Enum.Parse<GameAttributeType>(attributeKey);
            GameAttribute attribute = new GameAttribute(attributeType);

            return attribute;
        }

        public static GameAttribute[] FromJObject(JObject attributeObj)
        {
            List<GameAttribute> attributeList = new List<GameAttribute>();
            foreach(KeyValuePair<string, JToken> pair in attributeObj)
            {
                attributeList.Add(AttributeCreator.FromKey(pair.Key));
            }

            return attributeList.ToArray();
        }
        
    }
}
