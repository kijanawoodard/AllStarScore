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

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        //http://stackoverflow.com/a/222761/214073
        //Really dirty - and delicious
        public static T JsonCopy<T>(this T target)
        {
            return target.ToJson().FromJson<T>();
        }

        public static TResult NullOr<T, TResult>(this T foo, Func<T, TResult> func) where T : class
        {
            if (foo == null) return default(TResult);
            return func(foo);
        }
    }

    public static class DecimalExtension
    {
        public static decimal RoundUp(this decimal number, int places)
        {
            var result = Math.Round(number, places, MidpointRounding.AwayFromZero);
            return result;
        }
    }

}