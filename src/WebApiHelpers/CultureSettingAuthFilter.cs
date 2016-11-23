using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiHelpers.Contracts;

namespace WebApiHelpers
{
    public sealed class CultureSettingAuthFilter: IAsyncAuthorizationFilter, IOrderedFilter
    {        
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            CultureSetterUtilBase.SetCultureStatic(context.HttpContext);
            return Task.CompletedTask;
        }

        public int Order => int.MinValue;
    }
}
