using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrintMood.Config;
using PrintMood.RequestDTO;
using PrintMood.Resources;
using WebApiHelpers;
using WebApiHelpers.Contracts;
using WebApiHelpers.ReCaptcha;
using WebApiHelpers.XSS;

namespace PrintMood.Controllers
{    
    public class HomeController : Controller
    {
        private const string MailProfile = "MainContact";

        readonly IStringLocalizer _loc;
        readonly ISmtpServiceFactory _smtpFactory;
        readonly ILogger<HomeController> _logger;

        public HomeController (ILoggerFactory loggerFactory, ISmtpServiceFactory smtpFactory, ISharedResource sh)
        {
            _smtpFactory = smtpFactory;
            _loc = sh.Localizer;
            _logger = loggerFactory.CreateLogger<HomeController>();
        }

        [HttpGet]
        public IActionResult Index()
        {
            //throw new Exception("ex");            
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

        [HttpPost]
        [InvalidModelStateFilter]
        [ValidateAntiForgeryToken]
        [ValidateRecaptcha]
        [ValidateRequestXss]
        public async Task<IActionResult> SendMail( [FromForm] MailData md )
        {
            var mailService = _smtpFactory.Create(MailProfile);

            var msg = md.Message;
            if (!string.IsNullOrWhiteSpace(md.SiteUrl))
                msg += "\r\nContact Site: " + md.SiteUrl;

            try
            {
                await
                    mailService.Send(md.Email, md.Name,
                        $"Message from {md.Name}" +
                        (string.IsNullOrWhiteSpace(md.SiteUrl) ? string.Empty : $": {md.SiteUrl}"), msg);
            }
            catch (Exception ex)
            {
                var correlationId = HttpContext.TraceIdentifier;
                _logger.LogError(new EventId(1, correlationId), ex, $"Sending EMail through '{MailProfile}' failed: [{md.Name}: {md.Email}], text [{md.Message}]");
                return StatusCode((int) HttpStatusCode.InternalServerError, _loc["We are sorry, an error occurred while sending your message. To find out the cause you may contact the administartor and report the correlation number: {0}", correlationId].Value);
            }

            return Ok(_loc["Your message successfully sent"].Value);
        }
    }
}
