using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;


namespace WebApiHelpers.ReCaptcha
{
    public class ValidateRecaptchaFilter : IAsyncAuthorizationFilter
    {
        const string  FormField = "g-recaptcha-response";

        readonly IRecaptchaValidationService _service;
        readonly IStringLocalizer _loc;

        public ValidateRecaptchaFilter(IRecaptchaValidationService service, ISharedResource sr)
        {
            service.CheckArgumentNull(nameof(service));
            sr.CheckArgumentNull(nameof(sr));

            _service = service;
            _loc = sr.Localizer;
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
                        throw new RecaptchaValidationException($"Form data was not found in the POST request: {context.HttpContext.Request.ContentType}.", false);

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
                        context.Result = new BadRequestResult();
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
