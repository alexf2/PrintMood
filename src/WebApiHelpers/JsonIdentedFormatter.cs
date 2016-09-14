using Newtonsoft.Json;


namespace WebApiHelpers
{
    public class JsonIdentedFormatter: JsonDefaultFormatter
    {
        public JsonIdentedFormatter() 
        {
            SerializerSettings.Formatting = Formatting.Indented;
        }
    }
}
