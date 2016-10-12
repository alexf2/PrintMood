using Microsoft.Extensions.DependencyInjection;
using WebApiHelpers.Contracts;

namespace WebApiHelpers.ReCaptcha
{
    public static class RecaptchaServiceCollectionExtensions
    {
        public static IServiceCollection AddRecaptcha (this IServiceCollection services)
        {
            return services
                .AddSingleton<RecaptchaValidationService>()
                .AddSingleton<IRecaptchaValidationService>((sp) => sp.GetRequiredService<RecaptchaValidationService>())
                .AddSingleton<ValidateRecaptchaFilter>();
        }
    }
}
