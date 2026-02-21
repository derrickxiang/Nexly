using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.Messaging
{
    public class RabbitMqConsumer
    {
        private readonly IModel _channel;

        public RabbitMqConsumer(IConfiguration config)
        {
            var factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"],
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare("raw.news", true, false, false);
        }

        public void Consume(Func<string, Task> handler)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (s, e) =>
            {
                var json = Encoding.UTF8.GetString(e.Body.ToArray());
                await handler(json);
                _channel.BasicAck(e.DeliveryTag, false);
            };

            _channel.BasicConsume("raw.news", false, consumer);
        }
    }
}
