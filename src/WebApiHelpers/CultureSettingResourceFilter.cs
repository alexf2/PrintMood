using System.Globalization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace WebApiHelpers
{
    public sealed class CultureSettingResourceFilter : IResourceFilter, IOrderedFilter
    {
        const string ROUTE_CILTURE = "culture";

        public int Order => int.MinValue;

        public void OnResourceExecuted(ResourceExecutedContext context)
        {            
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {                             
            var culture = context.HttpContext.GetRouteData().Values["lang"]?.ToString();
            if (culture != null)
            {
                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);

                context.HttpContext.Items[ ROUTE_CILTURE ] = culture;
            }
            else if (context.HttpContext.Items[ROUTE_CILTURE] != null)
            {
                culture = (string)context.HttpContext.Items[ROUTE_CILTURE];

                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
            }
        }
    }
}
