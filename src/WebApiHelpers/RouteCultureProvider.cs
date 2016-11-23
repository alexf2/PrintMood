using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using WebApiHelpers.Contracts;

namespace WebApiHelpers
{
    public class RouteCultureProvider: RequestCultureProvider
    {
        readonly ICultureSetterUtil _util;
        public RouteCultureProvider(ICultureSetterUtil util)
        {
            _util = util;
        }

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");            

            var culture = _util.GetCulture(httpContext);

            if (string.IsNullOrEmpty(culture))
                return Task.FromResult<ProviderCultureResult>((ProviderCultureResult)null);
            
            return Task.FromResult<ProviderCultureResult>(new ProviderCultureResult(culture));
        }
    }
}
