using System;
using System.Linq;
using Microsoft.Extensions.Options;
using WebApiHelpers.Contracts;
using WebApiHelpers.SharedServices;

namespace PrintMood.Config
{
    public class SmtpServiceFactory: ISmtpServiceFactory
    {
        private readonly MailConfig _config;

        public SmtpServiceFactory (IOptions<MailConfig> config)
        {
            _config = config.Value;
        }

        public ISmtpService Create(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
                throw new ArgumentException($"{nameof(profileName)} is not specified");

            var cfg = _config.Profiles.FirstOrDefault(c => string.Compare(c.Profile, profileName, StringComparison.OrdinalIgnoreCase) == 0);
            if (cfg == null)
                throw new ArgumentException($"Mail profile '{profileName}' not found.");

            return new SmtpService(cfg.Settings, cfg.Description);
        }
    }
}
