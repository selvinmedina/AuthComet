namespace AuthComet.Mails.Common
{
    public class RabbitMqSettings
    {
        public string Uri { get; set; } = null!;
        public string QueueName { get; set; } = null!;
    }
}
