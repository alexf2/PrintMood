using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;


namespace WebApiHelpers
{
    /// <summary>
    /// Returns exceptions as JSON, in the response body, and sets http code to 500.
    /// </summary>
    public sealed class WebApiErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public WebApiErrorHandlingMiddleware(RequestDelegate next)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null)
                return;
            
            await WriteExceptionAsync(context, exception, HttpStatusCode.InternalServerError).ConfigureAwait(false);
        }

        static async Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;
            await response.WriteAsync(
                new ErrorResponseDto()
                {
                    Message = exception.Message,
                    Exception = exception.GetType().Name,
                    CorrelationId = context.TraceIdentifier,
                    HttpCode = (int)code
                }.Serialize()).ConfigureAwait(false);
        }

        public static IApplicationBuilder UseWebApiJsonErrorResponse (IApplicationBuilder bld)
        {
            bld.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = async (context) =>
                {
                    var ehf = context.Features.Get<IExceptionHandlerFeature>();
                    Exception exTmp = ehf == null ? new Exception("No exception") : ehf.Error;
                    await WebApiErrorHandlingMiddleware.WriteExceptionAsync(context, exTmp, HttpStatusCode.InternalServerError).ConfigureAwait(false);
                }
            });
            return bld;
        }
    }
}
