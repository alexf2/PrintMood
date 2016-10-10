using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiHelpers.Contracts;

namespace WebApiHelpers.SharedServices
{
    public sealed class SmtpService : ISmtpService
    {
        readonly IMailConfig _config;

        public SmtpService (IMailConfig config)
        {
            _config = config;
        }

        public async Task Send(string addressFrom, string subject, string body)
        {
            await Task.CompletedTask;
        }

        public async Task Send(string addressTo, string addressFrom, string addressReplyTo, string subject, string body)
        {
            await Task.CompletedTask;
        }
    }
}
