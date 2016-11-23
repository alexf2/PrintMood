using Microsoft.AspNetCore.Http;

namespace WebApiHelpers.Contracts
{
    public interface ICultureSetterUtil
    {
        string GetCulture(HttpContext context);        
        void SetCulture(HttpContext context);
    }
}
