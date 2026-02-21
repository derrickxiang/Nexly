using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Nexly.Worker.Crawler.Messaging
{
    public class EventPublisher : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EventPublisher()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "crawler-queue",
                durable: true,
                exclusive: false,
                autoDelete: false);
        }

        public void Publish(object message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: "",
                routingKey: "crawler-queue",
                basicProperties: null,
                body: body);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
