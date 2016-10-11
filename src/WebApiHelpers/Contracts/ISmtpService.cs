using System.Threading.Tasks;

namespace WebApiHelpers.Contracts
{
    public interface ISmtpService
    {
        Task Send(string addressFrom, string senderName, string subject, string body);

        Task Send(string addressTo, string addressFrom, string senderName, string addressReplyTo, string replyName, string subject, string body);
    }
}
