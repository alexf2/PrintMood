using System;

namespace WebApiHelpers.ReCaptcha
{
    public class RecaptchaValidationException : Exception
    {
        public bool InvalidResponse { get; }

        public RecaptchaValidationException(string message, bool invalidResponse) : base(message)
        {
            InvalidResponse = invalidResponse;
        }
    }
}
