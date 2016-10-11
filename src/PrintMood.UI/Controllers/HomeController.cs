using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
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
        private const string MailProfile = "Main";

        readonly IStringLocalizer _loc;
        readonly ISmtpServiceFactory _smtpFactory;

        public HomeController (ISmtpServiceFactory smtpFactory, SharedResource sh)
        {
            _smtpFactory = smtpFactory;
            _loc = sh.Localizer;
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

            await mailService.Send(md.Email, $"Message from {md.Name}", msg);            
            
            return Ok(_loc["Your message successfully sent"].Value);
        }
    }
}
