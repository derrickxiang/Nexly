using Nexly.Worker.Save.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.Save.Messaging
{
    public class RabbitMqListener
    {
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RabbitMqListener> _logger;

        public RabbitMqListener(
            IConfiguration config,
            IServiceScopeFactory scopeFactory,
            ILogger<RabbitMqListener> logger)
        {
            _config = config;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void StartListening()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:Host"]
            };

            var connection = factory.CreateConnectionAsync().Result;
            var channel = connection.CreateChannelAsync().Result;

            channel.QueueDeclareAsync(
                queue: "ai.result",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<SaveService>();

                    await service.SaveAsync(json);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving message");
                }
            };

            channel.BasicConsumeAsync(
                queue: "ai.result",
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation("Listening ai.result queue");
        }
    }
}
