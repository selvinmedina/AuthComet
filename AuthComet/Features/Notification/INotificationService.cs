
namespace AuthComet.Auth.Features.Notification
{
    public interface INotificationService
    {
        Task SendUserWhoLoggedIn(string user, string email);
    }
}