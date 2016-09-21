using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using PrintMood.ResponseDTO;
using WebApiHelpers;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PrintMood.ApiControllers
{
    public class RootController : Controller
    {
        public RootController()
        {            
        }

        /// <summary>
        /// Returns service information and version.
        /// </summary>
        /// <returns>ServiceInfo</returns>
        [Produces(typeof(ServiceInfo))]
        public IActionResult Get()
        {
            return Ok(new ServiceInfo()
            {
                ApplicationName = "PrintMood REST service",
                ApiVersion = VersionHelpers.GetProductVersion(GetType().GetTypeInfo().Assembly),
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
            return Ok($"Hello {name}!");
        }

        [HttpGet]
        public IActionResult SysInfo()
        {
            bool isWindows = false;
            bool isLinux = false;
            bool isMacOsX = false;

            string windir = Environment.GetEnvironmentVariable("windir");
            if (!string.IsNullOrEmpty(windir) && windir.Contains(@"\") && Directory.Exists(windir))            
                isWindows = true;            
            else if (System.IO.File.Exists(@"/proc/sys/kernel/ostype"))
            {
                string osType = System.IO.File.ReadAllText(@"/proc/sys/kernel/ostype");
                if (osType.StartsWith("Linux", StringComparison.OrdinalIgnoreCase))                
                    // Note: Android gets here too
                    isLinux = true;
                
                else                
                    throw new Exception("Unsupported OS");
                
            }
            else if (System.IO.File.Exists(@"/System/Library/CoreServices/SystemVersion.plist"))            
                // Note: iOS gets here too
                isMacOsX = true;            
            else            
                throw new Exception("Unsupported OS");
            

            var env = PlatformServices.Default.Application;
            return new ObjectResult(new
            {
                env.RuntimeFramework.FullName,
                env.RuntimeFramework.Identifier,
                vaersion = env.RuntimeFramework.Version.ToString(),
                isWindows,
                isLinux,
                isMacOsX
            });
        }
    }
}
