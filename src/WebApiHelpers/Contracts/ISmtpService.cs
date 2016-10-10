using System.Threading.Tasks;

namespace WebApiHelpers.Contracts
{
    public interface ISmtpService
    {
        Task Send(string addressFrom, string subject, string body);

        Task Send(string addressTo, string addressFrom, string addressReplyTo, string subject, string body);
    }
}
