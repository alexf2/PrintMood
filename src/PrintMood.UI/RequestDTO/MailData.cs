using System.ComponentModel.DataAnnotations;

namespace PrintMood.RequestDTO
{
    public sealed class MailData
    {
        [Required(ErrorMessage = "Your Name is missing")]
        [MinLength(5, ErrorMessage = "Your Name should not be shorter than 5 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Your Email is missing")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Url)]
        [Url]
        public string SiteUrl { get; set; }

        [Required(ErrorMessage = "The message is empty")]
        [MinLength(10, ErrorMessage = "Your Message should not be shorter than 10 characters")]
        public string Message { get; set; }
    }
}
