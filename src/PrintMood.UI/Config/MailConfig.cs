﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApiHelpers.Contracts;

namespace PrintMood.Config
{
    public sealed class MailConfig
    {
        public MailProfile[] Profiles { get; set; }    
    }

    public sealed class MailProfile
    {
        [Required]
        public string Profile { get; set; }

        [Required]
        public MailSettings Settings { get; set; }
    }

    public sealed class MailSettings: IMailConfig
    {        
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Bad format of ContactEMail")]
        public string ContactEMail { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Bad format of SysSourceEMail")]
        public string SysSourceEMail { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [Url(ErrorMessage = "SmtpHost address should be valid")]        
        public string SmtpHost { get; set; }

        [Range(0, 65535, ErrorMessage = "Smtp server port should be in range 0 - 65535")]
        [DefaultValue(25)]
        public int SmtpPort { get; set; } = 25;

        public string SmtLogin { get; set; }
        public string SmtPwd { get; set; }
    }
}
