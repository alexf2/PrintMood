using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiHelpers.ReCaptcha
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateRecaptchaAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        /// <inheritdoc />
        public bool IsReusable => true;

        /// <inheritdoc />
        public int Order { get; set; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<ValidateRecaptchaFilter>();
        }
    }
}
