namespace AuthComet.Mails.Features.Queues
{
    public static class RabbitMQExtensions
    {
        public static IApplicationBuilder UseRabbitMQListener(this IApplicationBuilder app)
        {
            var listener = app.ApplicationServices.GetService<RabbitMQConsumer>();

            if (listener is null)
            {
                throw new InvalidOperationException("RabbitMQ listener is not registered");
            }

            listener.InitializeRabbitMQ();

            return app;
        }
    }
}
