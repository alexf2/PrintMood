using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace WebApiHelpers
{
    public interface ISharedResource
    {
        IStringLocalizer Localizer { get; }
        IHtmlLocalizer HtmlLocalizer { get; }
    }
}
