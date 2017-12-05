using System.Threading.Tasks;
using WebApiHelpers.Contracts;

using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace WebApiHelpers.SharedServices
{
    public sealed class SmtpService : ISmtpService
    {
        readonly IMailConfig _config;
        readonly string _profileName;

        public SmtpService (IMailConfig config, string profileName)
        {
            _config = config;
            _profileName = profileName;
        }

        public async Task Send (string addressFrom, string senderName, string subject, string body)
        {
            await Send(_config.ContactEMail, _config.SysSourceEMail, _profileName, addressFrom, senderName, subject, body);
        }

        public async Task Send (string addressTo, string addressFrom, string senderName, string addressReplyTo, string replyName, string subject, string body)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(senderName, addressFrom));
            msg.To.Add(new MailboxAddress(string.Empty, addressTo));
            msg.ReplyTo.Add(new MailboxAddress(replyName, addressReplyTo));
            msg.Subject = subject;            
            msg.Body = new TextPart() {Text = body};

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_config.SmtpHost, _config.SmtpPort, SecureSocketOptions.None).ConfigureAwait(false);
                //await client.AuthenticateAsync(_config.SmtLogin, _config.SmtPwd);
                await client.SendAsync(msg).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
