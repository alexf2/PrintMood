using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PrintMood.RequestDTO;
using WebApiHelpers;

namespace PrintMood.Controllers
{
    //[ServiceFilter(typeof(LanguageActionFilter))]
    public class HomeController : Controller
    {
        readonly IOptions<MailConfig> _mailConfig;

        public HomeController(IOptions<MailConfig> mailConfig)
        {
            _mailConfig = mailConfig;
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
            return Ok($"The message successfully sent: {md.Name}");
        }
    }
}
