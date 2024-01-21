using Microsoft.AspNetCore.SignalR;

namespace AuthComet.Notification.Features.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotificationAboutUserWhoLoggedIn(string user, string email)
        {
            await Clients.All.SendAsync("UserLoggedIn", user, email);
        }
    }
}
