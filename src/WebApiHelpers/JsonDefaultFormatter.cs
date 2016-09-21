using System;
using System.Buffers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace WebApiHelpers
{
    public class JsonDefaultFormatter : JsonOutputFormatter
    {
        public JsonDefaultFormatter() : base(JsonSerializerSettingsProvider.CreateSerializerSettings(), ArrayPool<Char>.Shared)
        {
            SerializerSettings.Formatting = Formatting.None;

            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
            SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;                        
            SerializerSettings.TypeNameHandling = TypeNameHandling.None;
            SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = false });
            SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }        
    }
}
