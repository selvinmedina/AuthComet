namespace AuthComet.Mails.Features.Emails
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true);
        void SendEmail(string to, string subject, string body, bool isBodyHtml = true);
    }
}
