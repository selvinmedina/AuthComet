namespace AuthComet.Mails.Common.RabbitMessages
{
    public class UserCreationMessage
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
    }
}
