using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WebApiHelpers.ReCaptcha;

namespace PrintMood.Config
{
    public static class OptionsServiceCollectionExtensions
    {
        /// <summary>
        /// Loads configuration sections
        /// </summary>        
        public static IServiceCollection ConfigurePrintMoodApp (this IServiceCollection services, IConfigurationRoot conf)
        {
            return
                services.Configure<MailConfig>(conf.GetSection("MainConfig:Mail"))
                    .Configure<LocalizationConfig>(conf.GetSection("MainConfig:Localization"))
                    .Configure<RecaptchaOptions>(conf.GetSection("MainConfig:Recaptcha"));
        }
    }
}
