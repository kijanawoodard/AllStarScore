using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace AllStarScore.Extensions
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
                                                                    new IsoDateTimeConverter() //tODO: Blog - for knockout http://james.newtonking.com/archive/2009/02/20/good-date-times-with-json-net.aspx
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