using System.ComponentModel.DataAnnotations;


namespace PrintMood
{
    public sealed class MainConfigSection
    {
        public MailConfig Mail { get; set; }    
    }

    public sealed class MailConfig
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage= "Bad format of ContactEMail")]
        public string ContactEMail { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Bad format of SysSourceEMail")]
        public string SysSourceEMail { get; set; }

        [DataType(DataType.Url)]
        [Url(ErrorMessage= "SmtpHost address should be valid")]
        public string SmtpHost { get; set; }

        [Range(0, 65535, ErrorMessage="Smtp server port should be in range 0 - 65535")]
        public int SmtpPort { get; set; }
        
        public string SmtLogin { get; set; }
        public string SmtPwd { get; set; }
    }
}

