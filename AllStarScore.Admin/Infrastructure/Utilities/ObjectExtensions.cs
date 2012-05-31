using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AllStarScore.Admin.Infrastructure.Utilities
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.None, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        //try to convert to int or 0
        public static int ToInt(this string o)
        {
            int result;
            var ok = int.TryParse(o, out result);
            if (!ok) 
                result = 0;

            return result;
        }
    }
}