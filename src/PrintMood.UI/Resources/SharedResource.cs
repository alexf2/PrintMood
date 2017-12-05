using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using WebApiHelpers;
using WebApiHelpers.Contracts;

namespace PrintMood
{
    /// <summary>
    /// Resource access helper for shared resources.
    /// Put SharedResource.XX.resx besides this file.
    /// </summary>
    public sealed class SharedResource: ISharedResource
    {
        readonly IStringLocalizerFactory _stringFac;
        readonly IHtmlLocalizerFactory _htmlFac;
        readonly IHostingEnvironment _env;

        public SharedResource(IStringLocalizerFactory stringFac, IHtmlLocalizerFactory htmlFac, IHostingEnvironment env)
        {
            _stringFac = stringFac;
            _htmlFac = htmlFac;
            _env = env;
        }

        public IStringLocalizer Localizer => _stringFac.Create(nameof(SharedResource), _env.ApplicationName);
        //public IStringLocalizer Localizer => _stringFac.Create(typeof(SharedResource).GetType());

        public IHtmlLocalizer HtmlLocalizer => _htmlFac.Create(nameof(SharedResource), _env.ApplicationName);
    }
}
