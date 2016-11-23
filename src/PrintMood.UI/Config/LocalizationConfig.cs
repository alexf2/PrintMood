using Newtonsoft.Json;

namespace PrintMood.Config
{
    public sealed class LocalizationConfig
    {
        public sealed class LocaleInfo
        {
            [JsonProperty(PropertyName = "code")]
            public string Code { get; set; }

            [JsonProperty(PropertyName = "displayName")]
            public string DisplayName { get; set; }

            [JsonProperty(PropertyName = "specific")]
            public bool Specific { get; set; }
        }

        public sealed class DefaultLocaleConfig
        {
            [JsonProperty(PropertyName = "ui")]
            public string Ui { get; set; }

            [JsonProperty(PropertyName = "general")]
            public string General { get; set; }
        }

        [JsonProperty(PropertyName = "locales")]
        public LocaleInfo[] Locales { get; set; }

        [JsonProperty(PropertyName = "default")]
        public DefaultLocaleConfig Default { get; set; }
    }
}
