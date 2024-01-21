using AuthComet.Mails.Common;
using System.Net.Mail;
using System.Net;

namespace AuthComet.Mails.Features.Emails
{
    public class EmailService: IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(SmtpSettings smtpSettings, ILogger<EmailService> logger)
        {
            _smtpSettings = smtpSettings ?? throw new ArgumentNullException(nameof(smtpSettings));
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentException("Email recipient cannot be null or empty.", nameof(to));
            }

            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(to));
                message.From = new MailAddress(_smtpSettings.From);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isBodyHtml;

                using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    client.EnableSsl = _smtpSettings.EnableSsl;

                    try
                    {
                        await client.SendMailAsync(message);
                    }
                    catch (SmtpException smtpEx)
                    {
                        _logger.LogError(smtpEx, "Failed to send email.");
                        throw new InvalidOperationException("Failed to send email.", smtpEx);
                    }
                }
            }
        }
    }
}
