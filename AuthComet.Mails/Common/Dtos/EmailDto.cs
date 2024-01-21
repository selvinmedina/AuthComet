using System.ComponentModel.DataAnnotations;

namespace AuthComet.Mails.Common.Dtos
{
    public class EmailDto
    {
        [Required(ErrorMessage = "The 'To' field is required.")]
        [EmailAddress(ErrorMessage = "The 'To' field is not a valid email address.")]
        public string To { get; set; } = null!;

        [Required(ErrorMessage = "The 'Subject' field is required.")]
        [StringLength(100, ErrorMessage = "The 'Subject' field cannot exceed 100 characters.")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "The 'Body' field is required.")]
        public string Body { get; set; } = null!;

        public bool IsBodyHtml { get; set; } = true;
    }
}
