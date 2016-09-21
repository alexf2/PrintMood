using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace PrintMood.Resources
{
    /// <summary>
    /// Resource access helper for shared resources.
    /// Put Shared.XX.resx besides this file.
    /// </summary>
    public sealed class Shared
    {
        readonly IStringLocalizerFactory _stringFac;
        readonly IHtmlLocalizerFactory _htmlFac;
        readonly IHostingEnvironment _env;

        public Shared(IStringLocalizerFactory stringFac, IHtmlLocalizerFactory htmlFac, IHostingEnvironment env)
        {
            _stringFac = stringFac;
            _htmlFac = htmlFac;
            _env = env;
        }

        public IStringLocalizer Localizer => _stringFac.Create(nameof(Shared), null);

        public IHtmlLocalizer HtmlLocalizer => _htmlFac.Create(nameof(Shared), _env.ApplicationName);
    }
}
