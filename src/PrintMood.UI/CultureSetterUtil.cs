using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using PrintMood.Config;
using WebApiHelpers;

namespace PrintMood
{
    public sealed class CultureSetterUtil: CultureSetterUtilBase
    {
        readonly HashSet<string> _cultures;
        readonly string _defaultLocale;

        public CultureSetterUtil(IOptions<LocalizationConfig> localizationOptions)
        {            
            _cultures = new HashSet<string>(localizationOptions.Value.Locales.Where(loc => !loc.Specific).Select(loc => loc.Code));
            _defaultLocale = localizationOptions.Value.Default.Ui;
        }

        protected override bool IsCultureValid(string code)
        {
            return _cultures.Contains(code);
        }

        protected override string FallbackCulture => _defaultLocale;
    }
}
