using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PoEGraveList.Core.Misc
{
    static class RawQueryHelper
    {
        public static JObject GetTokenFromUrl(string url, string tokenName)
        {
            Uri queryUri = new Uri(url);
            NameValueCollection queryPairs = HttpUtility.ParseQueryString(queryUri.Query);
            string? rawToken = queryPairs.Get(tokenName);
            if (rawToken == null) throw new ArgumentException($"Provided URL does not contains \"{tokenName}\" parameter.");

            JObject? tokenObject = JsonConvert.DeserializeObject<JObject>(rawToken);
            if (tokenObject == null) throw new ArgumentException($"Can't parse query parameter \"{tokenName}\" as JSON object.");

            return tokenObject;
        }
    }
}
