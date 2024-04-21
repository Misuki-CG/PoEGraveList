using Newtonsoft.Json.Linq;
using PoEGraveList.Core.Attribute;
using PoEGraveList.Core.Misc;
using PoEGraveList.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoEGraveList.Core.Shop
{
    public static class ShopItemCreator
    {
        public static ShopItem[] FromLink(string link)
        {

            List<ShopItem> shopList = new List<ShopItem>();

            JObject rootObject = RawQueryHelper.GetTokenFromUrl(link, "gvc");
            foreach(KeyValuePair<string, JToken?> childToken in rootObject)
            {
                switch (childToken.Key)
                {
                    case "tiers":
                    case "prefix":
                    case "suffix":
                    case "explicit":
                    case "haunted":
                        ShopItem? item = FromSimpleTuple(childToken);
                        if (item == null) continue;
                        shopList.Add(item);
                        break;
                    case "weight":
                        ShopItem[] items = FromWeight((JObject)childToken.Value!);
                        shopList.AddRange(items);
                        break;
                    default:
                        continue;
                }
            }

            return [.. shopList];
        }

        private static ShopItem? FromSimpleTuple(KeyValuePair<string, JToken?> token)
        {

            if (token.Value != null && token.Value["0"] != null)
            {
                JProperty prop = (JProperty)token.Value["0"].First();
                int amount = int.Parse(prop.Value.ToString());
                bool isUpValue = int.Parse(prop.Name) >= 0;
                ShopItem item = new ShopItem()
                {
                    Attribute = AttributeCreator.Instance.FromKey(token.Key),
                    TotalAmount = amount,
                    IsUpValue = isUpValue
                };

                
                return item;
            }

            return null;
            
        }

        private static ShopItem[] FromWeight(JObject weightToken)
        {
            List<ShopItem> output = new List<ShopItem>();

            foreach(JToken token in weightToken.Children())
            {
                JProperty tokenProp = (JProperty)token;
                if (!tokenProp.HasValues) continue;
                if (!tokenProp.First!.HasValues) continue;
                JProperty attributeProp = (JProperty)tokenProp.First!.First!;

                string attributeKey = tokenProp.Name;
                bool isUpValue = int.Parse(attributeProp.Name) > 0;
                int amount = int.Parse(attributeProp.Value.ToString());

                ShopItem item = new ShopItem()
                {
                    Attribute = AttributeCreator.Instance.FromKey(attributeKey),
                    TotalAmount = amount,
                    IsUpValue = isUpValue
                };

                
                output.Add(item);

            }
            return [.. output];
        }

       
    }
}
