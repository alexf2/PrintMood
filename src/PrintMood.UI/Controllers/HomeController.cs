using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintMood.RequestDTO;
using WebApiHelpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PrintMood.Controllers
{
    [ServiceFilter(typeof(LanguageActionFilter))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {            
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
