using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiHelpers
{    
    public sealed class CultureSettingResourceFilter : IResourceFilter, IOrderedFilter
    {        
        public int Order => int.MinValue;

        public void OnResourceExecuted(ResourceExecutedContext context)
        {            
        }

        public void OnResourceExecuting(ResourceExecutingContext context)        
        {
            CultureSetterUtil.SetCulture(context.HttpContext);
        }
    }
}
