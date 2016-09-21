using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;


namespace WebApiHelpers
{
    public class JsonIdentedFormatter: JsonDefaultFormatter
    {
        public JsonIdentedFormatter() 
        {
            SerializerSettings.Formatting = Formatting.Indented;

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
