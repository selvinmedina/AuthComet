using AuthComet.Mails.Features.Emails;
using Microsoft.AspNetCore.SignalR.Client;

namespace AuthComet.Mails.Features.Notifications
{
    public class NotificationService : IAsyncDisposable
    {
        private HubConnection _hubConnection;
        private readonly IEmailService _emailService;

        public NotificationService(string hubUrl, IEmailService emailService)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            _emailService = emailService;

            _hubConnection.On<string, string>("UserLoggedIn", ReceiveUserWhoLoggedIn);
        }

        public void ReceiveUserWhoLoggedIn(string user, string email)
        {
            var subject = "Recent Login Notification";
            var body = GenerateLoginNotificationEmail(user);

            _emailService.SendEmail(email, subject, body, isBodyHtml: true);
        }

        private string GenerateLoginNotificationEmail(string username)
        {
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
                    <h1>Recent Login Detected</h1>
                    <p>Hello {username},</p>
                    <p>We detected a recent login to your account. If this was you, you can safely ignore this email. If you don't recognize this activity, please change your password immediately.</p>
                    <p><strong>Username:</strong> {username}</p>
            
                    <div class='footer'>
                        <p>Stay safe,<br>Your AuthComet Team</p>
                    </div>
                </div>
            </body>
            </html>";

            return htmlTemplate;
        }


        public async Task Start()
        {
            await _hubConnection.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            Console.WriteLine("DisposeAsync SignalRClient");
            await _hubConnection.DisposeAsync();
        }

        public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;
    }
}
