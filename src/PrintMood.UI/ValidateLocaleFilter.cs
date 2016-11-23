using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using PrintMood.Config;

namespace PrintMood
{
    public sealed class ValidateLocaleFilter: ActionFilterAttribute
    {
        readonly IOptions<LocalizationConfig> _localizationOptions;

        public ValidateLocaleFilter(IOptions<LocalizationConfig> localizationOptions) : base()
        {
            _localizationOptions = localizationOptions;
        }

        public override void OnActionExecuting (ActionExecutingContext context)
        {
            string locale;
            try
            {
                locale = context.ActionArguments["culture"] as string;
                if (locale == null)
                {
                    Return(context, 400, "Argument 'culture' is not specified.");
                    return;
                }
            }
            catch (Exception ex)
            {                
                Return(context, 400, "Argument 'culture' is not specified.");
                return;
            }

            if (_localizationOptions.Value.Locales.All(loc => loc.Code != locale))
            {
                Return(context, 400, $"Culture '{locale}' is not supported.");
                return;
            }

            base.OnActionExecuting(context);
        }

        static void Return (ActionExecutingContext ctx, int httpCode, string message)
        {
            ctx.HttpContext.Response.StatusCode = 400;
            ctx.Result = new ContentResult()
            {
                Content = message
            };
        }
    }
}
