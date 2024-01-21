using AuthComet.Mails.Features.Emails;

namespace AuthComet.Mails.Features.Statistics
{
    public class StatisticsService
    {
        private readonly IEmailService _emailService;

        public StatisticsService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendStatisticsAsync(string userName, string email)
        {
            var statistics = GenerateStatisticsForUser(userName);
            await _emailService.SendEmailAsync(email, "Your Weekly Statistics", statistics, true);
        }

        private string GenerateStatisticsForUser(string username)
        {
            // this is an example of a template that could be used to generate the statistics email

            var htmlTemplate = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                        .container {{ width: 80%; margin: 0 auto; background-color: #f7f7f7; padding: 20px; }}
                        h1 {{ color: #333366; }}
                        p {{ color: #555; }}
                        .footer {{ margin-top: 20px; font-size: 0.8em; text-align: center; color: #888; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Hello, {username}!</h1>
                        <p>Here are your weekly statistics:</p>
    
                        <h2>Recent Activity</h2>
                        <p>Details of your recent activity...</p>

                        <h2>Tips and Suggestions</h2>
                        <p>Some personalized tips and suggestions...</p>

                        <div class='footer'>
                            <p>Thank you for using AuthComet. We hope you enjoy your experience!</p>
                        </div>
                    </div>
                </body>
                </html>";

            return htmlTemplate;
        }

    }

}
