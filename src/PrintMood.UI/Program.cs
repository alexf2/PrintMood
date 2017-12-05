using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PrintMood
{
    //Core 1.0 - 2.0 migration guide: https://docs.microsoft.com/ru-ru/aspnet/core/migration/1x-to-2x/
    public class Program
    {
        public static void Main(string[] args)
        {
            /*var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();*/

            BuildWebHost(args).Run();
        }

        static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
