using System.Threading.Tasks;

namespace WebApiHelpers.Contracts
{
    public interface IRecaptchaValidationService
    {
        Task ValidateResponseAsync(string response, string remoteIp);

        string ValidationMessage { get; }
    }
}
