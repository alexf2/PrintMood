using System.Globalization;
#if NET451 || NET46
    using System.Threading;
#endif
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebApiHelpers
{
    public class LanguageActionFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public LanguageActionFilter (ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("LanguageActionFilter");
        }

        public override void OnActionExecuting (ActionExecutingContext context)
        {
            string culture = context.RouteData.Values["lang"].ToString();
            //_logger.LogInformation($"Setting the culture from the URL: {culture}");

#if NET451 || NET46
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
#else

            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
#endif

            base.OnActionExecuting(context);
        }
    }
}
