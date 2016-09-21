using System.Globalization;
#if NET451 || NET46
    using System.Threading;
#endif
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

#if NET451 || NET46
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
#else

                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
#endif                

                context.HttpContext.Items[ ROUTE_CILTURE ] = culture;
            }
            else if (context.HttpContext.Items[ROUTE_CILTURE] != null)
            {
                culture = (string)context.HttpContext.Items[ROUTE_CILTURE];

#if NET451 || NET46
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
#else

                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
#endif                                
            }
        }
    }
}
