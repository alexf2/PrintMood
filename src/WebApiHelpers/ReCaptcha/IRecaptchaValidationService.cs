using System.Threading.Tasks;

namespace WebApiHelpers.ReCaptcha
{
    public interface IRecaptchaValidationService
    {
        Task ValidateResponseAsync(string response, string remoteIp);

        string ValidationMessage { get; }
    }
}
