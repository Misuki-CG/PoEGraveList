using Newtonsoft.Json.Linq;
using PoEGraveList.Core.Misc;
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
        public static GameAttribute FromKeyPair(KeyValuePair<string, JToken?> attributeKeyPair)
        {
            GameAttributeType attributeType = Enum.Parse<GameAttributeType>(attributeKeyPair.Key);
            GameAttribute attribute = new GameAttribute(attributeType);
            if(attributeKeyPair.Value != null)
            {
                JProperty? pairValue = attributeKeyPair.Value.First as JProperty;
                if(pairValue != null)
                {
                    attribute.Level = Enum.Parse<GameAttributeLevel>(pairValue.Name);
                    attribute.Amount = int.Parse(pairValue.Value.ToString());
                }
            }
            return attribute;
        }

        public static GameAttribute[] FromJObject(JObject attributeObj)
        {
            List<GameAttribute> attributeList = new List<GameAttribute>();
            foreach(KeyValuePair<string, JToken?> pair in attributeObj)
            {
                attributeList.Add(AttributeCreator.FromKeyPair(pair));
            }

            return attributeList.ToArray();
        }
        
        public static GameAttribute[] FromLink(string queryLink)
        {
            JObject rootToken = RawQueryHelper.GetTokenFromUrl(queryLink, "gvc");
            JObject? weightToken = rootToken.Value<JObject>("weight");

            if (weightToken == null) return [];

            GameAttribute[] weightList = AttributeCreator.FromJObject(weightToken);
            return weightList;
        }
    }
}
