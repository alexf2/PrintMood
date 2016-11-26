using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WebApiHelpers.Contracts;

namespace PrintMood.Controllers
{
    public class HomeController : Controller
    {        
        readonly IStringLocalizer _loc;        
        readonly ILogger<HomeController> _logger;

        public HomeController (ILoggerFactory loggerFactory, ISharedResource sh)
        {            
            _loc = sh.Localizer;
            _logger = loggerFactory.CreateLogger<HomeController>();            
        }

        [HttpGet]
        public IActionResult Index()
        {            
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        //https://damienbod.com/2016/09/09/asp-net-core-action-arguments-validation-using-an-actionfilter/
        [HttpGet]
        [ServiceFilter(typeof(ValidateLocaleFilter))]
        public IActionResult SetCulture (string culture)
        {
            if (culture == "en")
                Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
            else
                Response.Cookies.Append(
                      CookieRequestCultureProvider.DefaultCookieName,
                      CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                      new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1)}
                );
            return RedirectToRoute("withlang", new { lang = culture, controller = "Home", action = "Index" });
            //return Ok(Url.Link("withlang", new {lang = culture, controller = "Home", action = "Index"}));
        }
    }
}
