
namespace WebApiHelpers.Contracts
{
    public interface IMailConfig
    {
        string ContactEMail { get; set; }
        
        string SysSourceEMail { get; set; }
        
        string SmtpHost { get; set; }
        
        int SmtpPort { get; set; }

        string SmtLogin { get; set; }
        string SmtPwd { get; set; }
    }
}
