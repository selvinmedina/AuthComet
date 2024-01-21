using AuthComet.Mails.Common;
using AuthComet.Mails.Common.RabbitMessages;
using AuthComet.Mails.Features.Emails;
using AuthComet.Mails.Features.Statistics;
using Hangfire;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace AuthComet.Mails.Features.Queues
{
    public class RabbitMQConsumer : IDisposable
    {
        private readonly RabbitMqSettings _rabbitMqSettings;
        private IConnection? _connection;
        private IModel? _channel;
        private readonly IEmailService _emailService;

        public RabbitMQConsumer(RabbitMqSettings rabbitMqSettings, IEmailService emailService)
        {
            _rabbitMqSettings = rabbitMqSettings;
            _emailService = emailService;
        }

        public void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_rabbitMqSettings.Uri) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _rabbitMqSettings.QueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnConsumerReceived;

            _channel.BasicConsume(queue: _rabbitMqSettings.QueueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void OnConsumerReceived(object? model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<UserCreationMessage>(messageString);

            if (message != null)
            {
                ProcessMessage(message);
            }
        }

        private void ProcessMessage(UserCreationMessage message)
        {
            var recurringJobId = $"SendStatistics-{message.UserId}";

            RecurringJob.AddOrUpdate<StatisticsService>(
                recurringJobId,
                x => x.SendStatisticsAsync(message.Username, message.Email),
                Cron.Weekly);
        }


        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
