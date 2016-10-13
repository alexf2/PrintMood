using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiHelpers
{
    public sealed class CultureSettingAuthFilter: IAsyncAuthorizationFilter, IOrderedFilter
    {        
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            CultureSetterUtil.SetCulture(context.HttpContext);
            return Task.CompletedTask;
        }

        public int Order => int.MinValue;
    }
}
