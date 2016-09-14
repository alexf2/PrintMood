using System;
using System.Buffers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiHelpers
{
    public class JsonDefaultFormatter : JsonOutputFormatter
    {
        public JsonDefaultFormatter() : base(JsonSerializerSettingsProvider.CreateSerializerSettings(), ArrayPool<Char>.Shared)
        {
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
        }

        protected override bool CanWriteType(Type type)
        {
            return base.CanWriteType(type);
        }
    }
}
