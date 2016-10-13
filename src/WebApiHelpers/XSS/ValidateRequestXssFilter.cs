using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using WebApiHelpers.Contracts;

namespace WebApiHelpers.XSS
{
    public sealed class ValidateRequestXssFilter: IAsyncAuthorizationFilter, IOrderedFilter
    {
        readonly Regex _regXss = new Regex("(javascript[^*(%3a)]*(%3a|:))|(%3C*|<)[^*]?script|(document*(%2e|.))|(setInterval[^*(%28)]*(%28|\\())|(setTimeout[^*(%28)]*(%28|\\())|(alert[^*(%28)]*(%28|\\())|(((\\%3C) <)[^\n]+((\\%3E) >))", RegexOptions.IgnoreCase);

        readonly IStringLocalizer _loc;

        public int Order => int.MinValue + 1;

        public ValidateRequestXssFilter (ISharedResource sr)
        {
            _loc = sr.Localizer;
        }

        public Task OnAuthorizationAsync (AuthorizationFilterContext context)
        {            
            context.CheckArgumentNull(nameof(context));
            context.HttpContext.CheckArgumentNull(nameof(context.HttpContext));

            if (context.HttpContext != null && context.HttpContext.Request != null)
            {
                var q = context.HttpContext.Request.QueryString.ToString();
                if (!string.IsNullOrEmpty(q) && _regXss.IsMatch(q))
                {
                    context.Result = new BadRequestObjectResult(new ErrorResponseDto()
                    {
                        Message = _loc["Query string possible contains malicious code"].Value,
                        CorrelationId = context.HttpContext.TraceIdentifier,
                        HttpCode = (int)HttpStatusCode.BadRequest
                    });

                    return Task.CompletedTask;
                }

                if (context.HttpContext.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
                    context.HttpContext.Request.HasFormContentType)
                {
                    var f = context.HttpContext.Request.Form;
                    string k;
                    if (f.Count > 0 && IsMatch(f, out k))
                    {
                        context.Result = new BadRequestObjectResult(new ErrorResponseDto()
                        {
                            Message = _loc["Posted form data possible contains malicious code in '{0}' field", k].Value,
                            CorrelationId = context.HttpContext.TraceIdentifier,
                            HttpCode = (int)HttpStatusCode.BadRequest
                        });

                        return Task.CompletedTask;
                    }
                }

            }

            return Task.CompletedTask;
        }

        bool IsMatch (IFormCollection f, out string dangerousKey)
        {
            dangerousKey = string.Empty;

            foreach (var k in f.Keys)
                if (_regXss.IsMatch(f[k]))
                {
                    dangerousKey = k;
                    return true;
                }

            return false;
        }
    }
}
