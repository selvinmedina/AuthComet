using AuthComet.Auth.Common;
using AuthComet.Auth.Common.RabbitMessages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AuthComet.Auth.Features.Queues
{
    public class RabbitMQProducer
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly RabbitMqSettings _rabbitMqSettings;

        public RabbitMQProducer(RabbitMqSettings settings)
        {
            _rabbitMqSettings = settings;
            _factory = new ConnectionFactory() { Uri = new(settings.Uri) };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateChannel();

            _channel.QueueDeclare(queue: settings.QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void SendMessage(UserCreationMessage message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: "",
                routingKey: _rabbitMqSettings.QueueName,
                body: body);
        }
    }
}
