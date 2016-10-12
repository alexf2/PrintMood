using System;
using System.Net.Http;

namespace WebApiHelpers.ReCaptcha
{
    public sealed class RecaptchaOptions
    {
        public string ResponseValidationEndpoint { get; set; } = "https://www.google.com/recaptcha/api/siteverify";

        public string SecretKey { get; set; }

        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        public TimeSpan BackchannelTimeout { get; set; } = TimeSpan.FromSeconds(60);
        
        public string ValidationMessage { get; set; }      
    }
}
