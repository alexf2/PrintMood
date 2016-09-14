using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebApiHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;

namespace PrintMood
{
    public sealed class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices (IServiceCollection services)
        {
            services
                .AddElm(opt =>
                {
                    opt.Path = new PathString("/elm");
                    opt.Filter = (name, level) => level >= LogLevel.Error;
                });
                   
            services.AddLogging()
                .AddMvc(opt =>
                {
                    opt.RespectBrowserAcceptHeader = true;
                    opt.OutputFormatters.RemoveType<JsonOutputFormatter>();
                    opt.OutputFormatters.Insert(0, new JsonIdentedFormatter());
                    opt.OutputFormatters.Insert(0, new JsonDefaultFormatter());
                })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            services.Replace(ServiceDescriptor.Singleton<ObjectResultExecutor, ObjectResultExecutorWithIndent>());            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //ObjectResultExecutor ex = (ObjectResultExecutor)app.ApplicationServices.GetService(typeof (ObjectResultExecutor));
            
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"))
                    .AddDebug(LogLevel.Debug);

                app.UseDeveloperExceptionPage();
            }
            else
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                app.UseExceptionHandler("/Home/Error");

                app.UseElmCapture();
                app.UseElmPage();
            }

            var logger = loggerFactory.CreateLogger<Startup>();

            logger.LogWarning($"The environment is dev: {env.IsDevelopment()}");

            logger.LogInformation("Is about running");
            
            app.UseRequestLocalization()
                .UseStaticFiles()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
            

            app.Map("/api", bld =>
            {           
                     
                bld.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Root}/{action=Get}")

                        .MapRoute(
                            name: "RpcRoute",
                            template: "{controller}/{action}/{name?}");
                });
            });

            /*app.Run(async (context) =>
            {
                logger.LogInformation("Is running");
                await context.Response.WriteAsync("Hello World!");
            });*/
        }
    }
}
