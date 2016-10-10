using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using PrintMood.Config;
using PrintMood.RequestDTO;
using WebApiHelpers;
using WebApiHelpers.Contracts;

namespace PrintMood.Controllers
{    
    public class HomeController : Controller
    {
        private const string MailProfile = "Main";

        readonly ISmtpServiceFactory _smtpFactory;
        readonly IStringLocalizer<HomeController> _localizerizer;

        private IStringLocalizerFactory _stringFac;

        public HomeController (ISmtpServiceFactory smtpFactory, IStringLocalizer<HomeController> localizerizer, IStringLocalizerFactory stringFac)
        {
            _smtpFactory = smtpFactory;
            _localizerizer = localizerizer;
            _stringFac = stringFac;
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

            StringLocalizer<HomeController> ll = null;
            var cc = _stringFac.Create(this.GetType());

            Microsoft.Extensions.Localization.ResourceManagerStringLocalizer hh = null;

            return Ok(_localizerizer["Your message successfully sent"].Value);
        }
    }
}
