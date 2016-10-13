using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiHelpers
{
    public sealed class ErrorResponseDto
    {
        static readonly JsonSerializerSettings _settings = new JsonSerializerSettings() { Formatting  = Formatting.None, TypeNameHandling  = TypeNameHandling.None, ContractResolver  = new CamelCasePropertyNamesContractResolver()};

        public string Message { get; set; }

        public string Exception { get; set; }

        public string CorrelationId { get; set; }

        public int HttpCode { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject( new {Error = this}, _settings);
        }
    }
}
