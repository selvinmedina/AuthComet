using Microsoft.AspNetCore.SignalR.Client;

namespace AuthComet.Auth.Features.Notification
{
    public class NotificationService : IAsyncDisposable, INotificationService
    {
        private HubConnection _hubConnection;

        public NotificationService(string hubUrl)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();
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

        public async Task SendUserWhoLoggedIn(string user, string email)
        {
            try
            {
                if (!IsConnected)
                {
                    await _hubConnection.StartAsync();
                }

                await _hubConnection.SendAsync("SendNotificationAboutUserWhoLoggedIn", user, email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
