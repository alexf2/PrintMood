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

        public Task<IViewComponentResult> InvokeAsync ()
        {            
            return Task.FromResult<IViewComponentResult>(View(_localizationOptions.Value));
        }
    }

    
}
