using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiHelpers.Contracts
{
    public interface ISmtpServiceFactory
    {
        ISmtpService Create(string profileName);
    }
}
