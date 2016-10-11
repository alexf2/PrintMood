using System;
using System.Net;
using System.Threading.Tasks;
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

namespace PrintMood.Controllers
{    
    public class HomeController : Controller
    {
        private const string MailProfile = "MainContact";

        readonly IStringLocalizer _loc;
        readonly ISmtpServiceFactory _smtpFactory;
        readonly ILogger<HomeController> _logger;

        public HomeController (ILoggerFactory loggerFactory, ISmtpServiceFactory smtpFactory, SharedResource sh)
        {
            _smtpFactory = smtpFactory;
            _loc = sh.Localizer;
            _logger = loggerFactory.CreateLogger<HomeController>();
        }

        public IActionResult Index()
        {
            //throw new Exception("ex");            
            return View();
        }        

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        [InvalidModelStateFilter]
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
