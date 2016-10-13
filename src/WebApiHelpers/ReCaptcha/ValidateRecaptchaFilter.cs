using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WebApiHelpers.Contracts;


namespace WebApiHelpers.ReCaptcha
{
    public sealed class ValidateRecaptchaFilter : IAsyncAuthorizationFilter, IOrderedFilter
    {
        const string  FormField = "g-recaptcha-response";

        readonly IRecaptchaValidationService _service;
        readonly IStringLocalizer _loc;
        readonly ILogger<ValidateRecaptchaFilter> _logger;

        public int Order => int.MinValue + 2;

        public ValidateRecaptchaFilter(IRecaptchaValidationService service, ISharedResource sr, ILoggerFactory logFac)
        {
            service.CheckArgumentNull(nameof(service));
            sr.CheckArgumentNull(nameof(sr));

            _service = service;
            _loc = sr.Localizer;
            _logger = logFac.CreateLogger<ValidateRecaptchaFilter>();
        }

        /// <inheritdoc />
        public async Task OnAuthorizationAsync (AuthorizationFilterContext context)
        {
            context.CheckArgumentNull(nameof(context));
            context.HttpContext.CheckArgumentNull(nameof(context.HttpContext));

            if (ShouldValidate(context))
            {                
                Action invalidResponse = () => context.ModelState.AddModelError(FormField, _service.ValidationMessage);

                try
                {
                    if (!context.HttpContext.Request.HasFormContentType)
                        throw new RecaptchaValidationException(_loc["Form data was not found in the POST request: {0}", context.HttpContext.Request.ContentType].Value, false);

                    var form = await context.HttpContext.Request.ReadFormAsync();
                    var response = form[FormField];
                    var remoteIp = context.HttpContext.Connection?.RemoteIpAddress?.ToString();


                    if (string.IsNullOrEmpty(response))
                    {
                        invalidResponse();
                        return;
                    }

                    await _service.ValidateResponseAsync(response, remoteIp);
                }
                catch (RecaptchaValidationException ex)
                {
                    if (ex.InvalidResponse)
                    {
                        invalidResponse();
                        return;
                    }
                    else
                    {
                        _logger.LogError(new EventId(1, context.HttpContext.TraceIdentifier), ex, "At validating captcha");
                        context.Result = new BadRequestObjectResult(new ErrorResponseDto()
                        {
                            Message = ex.Message,                            
                            CorrelationId = context.HttpContext.TraceIdentifier,
                            HttpCode = (int)HttpStatusCode.BadRequest
                        });
                    }
                }
            }
        }

        bool ShouldValidate(AuthorizationFilterContext context)
        {
            return string.Equals("POST", context.HttpContext.Request.Method, StringComparison.OrdinalIgnoreCase);
        }
    }
}
