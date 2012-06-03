using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace AllStarScore.Admin.Infrastructure.Utilities
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.None,
                                               new JsonSerializerSettings()
                                               {
                                                   ContractResolver = new CamelCasePropertyNamesContractResolver(), 
                                                   Converters = new List<JsonConverter>
                                                                {
                                                                    new IsoDateTimeConverter() //tODO: Blog - for knockout
                                                                }
                                               });
        }

        public static TResult NullOr<T, TResult>(this T foo, Func<T, TResult> func) where T : class
        {
            if (foo == null) return default(TResult);
            return func(foo);
        }
    }
}