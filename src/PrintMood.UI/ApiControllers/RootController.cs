using System;
using Microsoft.AspNetCore.Mvc;
using PrintMood.ResponseDTO;
using WebApiHelpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PrintMood.ApiControllers
{
    public class RootController : Controller
    {                
        /// <summary>
        /// Returns service information and version.
        /// </summary>
        /// <returns>ServiceInfo</returns>
        [Produces(typeof(ServiceInfo))]
        public IActionResult Get()
        {
            return Ok(new ServiceInfo()
            {
                ApplicationName = "Rambler.Cinema service",
                ApiVersion = VersionHelpers.GetProductVersion(GetType().Assembly),
                Links = new { Self = Url.Link("default", null) }
            });
        }

        /// <summary>
        /// A test hello methods, which return an echo.
        /// </summary>
        /// <param name="name">An arbitrary word to echo</param>
        /// <returns>Hello {name}!</returns>
        [HttpGet]
        public IActionResult Hello (string name)
        {
            throw new Exception("test API");
            return Ok($"Hello {name}!");
        }
    }
}
