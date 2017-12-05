using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
#if NET451 || NET46 
using System.Threading;
#endif
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;// Globalization;
using WebApiHelpers.Contracts;

namespace WebApiHelpers
{    
    public abstract class CultureSetterUtilBase: ICultureSetterUtil
    {
        const string ROUTE_CILTURE = "culture";

        protected abstract bool IsCultureValid (string code);
        protected abstract string FallbackCulture { get; }

        string ICultureSetterUtil.GetCulture(HttpContext context)
        {            
            if (context.Request.Path.HasValue)
            {
                var components = context.Request.Path.Value.Split(new char['/'], StringSplitOptions.RemoveEmptyEntries);
                if (components.Length > 0)
                {
                    
                    var comp = components[0].Trim().Replace("/", string.Empty);
                    if (comp.Length == 2 && IsCultureValid(comp))
                        return comp;
                }                
            }
            return null;
        }

        static string GetCulture(HttpContext context) => context.GetRouteData().Values["lang"]?.ToString().Trim().ToLower();

        static void SetCultureInternal(HttpContext context, string culture)
        {
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

        public void SetCulture(HttpContext context)
        {
            var culture = GetCulture(context);
            if (!IsCultureValid(culture))
                culture = FallbackCulture;

            SetCultureInternal(context, culture);
        }

        public static void SetCultureStatic(HttpContext context) => SetCultureInternal(context, GetCulture(context));        
    }
}
