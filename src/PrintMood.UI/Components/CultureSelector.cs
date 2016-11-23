using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PrintMood.Config;

namespace PrintMood.Components
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CultureSelector: ViewComponent
    {
        readonly IOptions<LocalizationConfig> _localizationOptions;

        public CultureSelector (IOptions<LocalizationConfig> localizationOptions)
        {
            _localizationOptions = localizationOptions;
        }

        public async Task<IViewComponentResult> InvokeAsync ()
        {            
            return View(_localizationOptions.Value);
        }
    }

    
}
