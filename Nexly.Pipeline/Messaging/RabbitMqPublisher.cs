using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Nexly.Pipeline.Messaging
{
    public class RabbitMqPublisher
    {
        private readonly RabbitMQ.Client.IModel _channel;

        public RabbitMqPublisher(IConfiguration config)
        {
            var factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"]
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public Task PublishAsync<T>(string queue, T message)
        {
            _channel.QueueDeclare(queue, true, false, false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish("", queue, null, body);

            return Task.CompletedTask;
        }
    }
}
