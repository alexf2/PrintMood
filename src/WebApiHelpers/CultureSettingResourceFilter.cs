using Microsoft.AspNetCore.Mvc.Filters;
using WebApiHelpers.Contracts;

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
            CultureSetterUtilBase.SetCultureStatic(context.HttpContext);
        }
    }
}
