using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using WebApiHelpers;
using WebApiHelpers.Contracts;

namespace PrintMood.Config
{
    /// <summary>
    /// Configures localization. Should be used in void ConfigureServices (IServiceCollection services).
    /// </summary>
    internal sealed class ConfigureLocales: IConfigureOptions<RequestLocalizationOptions>
    {
        readonly IOptions<LocalizationConfig> _locales;
        readonly ICultureSetterUtil _util;

        public ConfigureLocales (IOptions<LocalizationConfig> locales, ICultureSetterUtil util)
        {
            _locales = locales;
            _util = util;
        }

        public void Configure (RequestLocalizationOptions options)
        {            
            var supportedCultures = _locales.Value.Locales.Select(loc => new CultureInfo(loc.Code)).ToList();

            options.DefaultRequestCulture = new RequestCulture(culture: _locales.Value.Default.General, uiCulture: _locales.Value.Default.Ui);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new RouteCultureProvider(_util),
                new CookieRequestCultureProvider()
            };
        }
    }
}
