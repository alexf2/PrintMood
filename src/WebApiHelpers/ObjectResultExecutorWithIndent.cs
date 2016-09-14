using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApiHelpers
{
    public class ObjectResultExecutorWithIndent: ObjectResultExecutor
    {
        public ObjectResultExecutorWithIndent(IOptions<MvcOptions> options, IHttpResponseStreamWriterFactory writerFactory,
            ILoggerFactory loggerFactory): base(options,  writerFactory, loggerFactory)
        {            
        }

        protected override IOutputFormatter SelectFormatter(OutputFormatterWriteContext formatterContext,
            MediaTypeCollection contentTypes, IList<IOutputFormatter> formatters)
        {
            var res = base.SelectFormatter(formatterContext, contentTypes, formatters);
            if (res is JsonDefaultFormatter && formatterContext.HttpContext.Request.Query.ContainsKey("idented"))
            {
                var fmtIdented = formatters.FirstOrDefault(f => f.GetType() == typeof(JsonIdentedFormatter));
                if (fmtIdented != null)
                    res = fmtIdented;
            }

            return res;
        }
    }
}
