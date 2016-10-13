using Microsoft.Extensions.DependencyInjection;

namespace WebApiHelpers.XSS
{
    public static class XssServiceCollectionExtensions
    {
        public static IServiceCollection AddXssValidation(this IServiceCollection services)
        {
            return services.AddSingleton<ValidateRequestXssFilter>();
        }
    }
}
