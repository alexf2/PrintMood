using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebApiHelpers;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using PrintMood.Resources;

namespace PrintMood
{
    public sealed class Startup
    {
        const string StartKey = "Startup Error";
        const string ConfigKey = "Configuration Error";

        readonly Dictionary<string, List<ExceptionDispatchInfo>> _errors = new Dictionary<string, List<ExceptionDispatchInfo>>()
        {
            {StartKey, new List<ExceptionDispatchInfo>() },
            {ConfigKey, new List<ExceptionDispatchInfo>() }
        };

        public Startup(IHostingEnvironment env)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();
                Configuration = builder.Build();             
            }
            catch (Exception ex)
            {
                _errors[StartKey].Add(ExceptionDispatchInfo.Capture(ex));
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices (IServiceCollection services)
        {
            try
            {
                services
                    .AddLocalization(options => options.ResourcesPath = "Resources")
                
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
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();

                //Substitutes Json formatters rcognizing "idented" query string parameter
                services.Replace(ServiceDescriptor.Singleton<ObjectResultExecutor, ObjectResultExecutorWithIndent>());

                
                services.AddScoped<LanguageActionFilter>();

                services.AddSingleton<Shared>();
                    

                services.Configure<RequestLocalizationOptions>(
                    options =>
                    {
                        var supportedCultures = new List<CultureInfo>
                            {
                                new CultureInfo("en-US"),
                                new CultureInfo("ru-RU"),
                                new CultureInfo("sk-SK"),

                                new CultureInfo("en"),
                                new CultureInfo("ru"),
                                new CultureInfo("sk")
                            };

                        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                        options.SupportedCultures = supportedCultures;
                        options.SupportedUICultures = supportedCultures;
                    });
            }
            catch (Exception ex)
            {
                _errors[ConfigKey].Add(ExceptionDispatchInfo.Capture(ex));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

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
            app.UseStatusCodePagesWithReExecute("/Home/Error");

            var logger = loggerFactory.CreateLogger<Startup>();

            logger.LogWarning($"The environment is dev: {env.IsDevelopment()}");

            logger.LogInformation("Is about running");
            
            if (_errors.Any(p => p.Value.Any()))
            {
                ReturnErrors(app, logger);
                return;
            }
            
            app.UseRequestLocalization()
                .UseStaticFiles()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}",
                        defaults: new {lang="en"},
                        constraints: null,
                        dataTokens: new {Namespace = typeof (PrintMood.Controllers.HomeController).Namespace}
                        )
                        .MapRoute(
                            name: "withlang",
                            template: "{lang=en}/{controller=Home}/{action=Index}/{id?}",
                            defaults: null,
                            constraints: new {lang = "en|ru|sk"},
                            dataTokens: new {Namespace = typeof (PrintMood.Controllers.HomeController).Namespace}
                        );
                });

            app.UseElmCapture();
            app.UseElmPage();

            app.Map("/api", bld =>
            {
                WebApiErrorHandlingMiddleware.UseWebApiJsonErrorResponse(bld)               
                    .UseMvc(routes =>
                    {
                        routes.MapRoute(
                            name: "default",
                            template: "{controller=Root}/{action=Get}",
                            defaults: null,
                            constraints: null,
                            dataTokens: new {Namespace = typeof (PrintMood.ApiControllers.RootController).Namespace})

                            .MapRoute(
                                name: "RpcRoute",
                                template: "{controller}/{action}/{name?}",
                                defaults: null,
                                constraints: null,
                                dataTokens: new {Namespace = typeof (PrintMood.ApiControllers.RootController).Namespace});
                    });
            });            
        }

        void ReturnErrors(IApplicationBuilder app, ILogger logger)
        {            
            app.Run(
                async ctx =>
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    ctx.Response.ContentType = "text/plain";

                    ctx.Response.Headers["Cache-Control"] = (StringValues)"no-cache";
                    ctx.Response.Headers["Pragma"] = (StringValues)"no-cache";
                    ctx.Response.Headers["Expires"] = (StringValues)"-1";
                    ctx.Response.Headers.Remove("ETag");

                    foreach (var ex in _errors)
                        foreach (var val in ex.Value)
                        {
                            try
                            {
                                val.Throw();
                            }
                            catch (Exception exc)
                            {
                                logger.LogCritical($"{ex.Key}:::{exc}");
                                await ctx.Response.WriteAsync($"{ex.Key}: {exc}\r\n").ConfigureAwait(false);
                            }                            
                        }
                }
            );
        }
    }
}
