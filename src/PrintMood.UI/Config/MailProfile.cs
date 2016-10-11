using System.ComponentModel.DataAnnotations;


namespace PrintMood.Config
{
    public sealed class MailProfile
    {
        [Required]
        public string Profile { get; set; }

        public string Description { get; set; }

        [Required]
        public MailSettings Settings { get; set; }
    }
}
