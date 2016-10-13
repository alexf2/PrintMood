using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
#if NET451 || NET46
    using System.Threading;
#endif
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace WebApiHelpers
{
    public static class CultureSetterUtil
    {
        const string ROUTE_CILTURE = "culture";

        public static void SetCulture(HttpContext context)
        {
            var culture = context.GetRouteData().Values["lang"]?.ToString();
            if (culture != null)
            {

#if NET451 || NET46
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
#else

                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
#endif                

                context.Items[ROUTE_CILTURE] = culture;
            }
            else if (context.Items[ROUTE_CILTURE] != null)
            {
                culture = (string)context.Items[ROUTE_CILTURE];

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
