namespace PrintMood.Config
{
    public sealed class LocalizationConfig
    {
        public string[] Locales { get; set; }
        public DefaultLocaleConfig Default { get; set; }
    }
}
