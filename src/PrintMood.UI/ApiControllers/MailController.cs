using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using PrintMood.RequestDTO;
using WebApiHelpers;
using WebApiHelpers.Contracts;
using WebApiHelpers.ReCaptcha;
using WebApiHelpers.XSS;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PrintMood.ApiControllers
{
    public class MailController : Controller
    {
        private const string MailProfile = "MainContact";
        public const string ControllerName = "Mail";

        readonly IStringLocalizer _loc;
        readonly ISmtpServiceFactory _smtpFactory;
        readonly ILogger<MailController> _logger;

        public MailController(ILoggerFactory loggerFactory, ISmtpServiceFactory smtpFactory, ISharedResource sh)
        {
            _smtpFactory = smtpFactory;
            _loc = sh.Localizer;
            _logger = loggerFactory.CreateLogger<MailController>();
        }

        [HttpPost]
        [InvalidModelStateFilter]
        [ValidateAntiForgeryToken]
        [ValidateRecaptcha]
        [ValidateRequestXss]
        public async Task<IActionResult> SendMail([FromForm] MailData md)
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
                return new ContentResult()
                {
                    Content =
                        _loc[
                            "We are sorry, an error occurred while sending your message. To find out the cause you may contact the administartor and report the correlation number: {0}",
                            correlationId].Value,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            return Ok(new
            {
                message =_loc["Your message successfully sent"].Value,
                Links = new
                {
                    Self = Url.Action("SendMail", MailController.ControllerName, new MailData(), Request.Scheme)
                }
            });
        }
    }
}
