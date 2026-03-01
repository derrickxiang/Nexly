using Microsoft.EntityFrameworkCore.Metadata;
using Nexly.Messaging.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nexly.Messaging.Services
{
    public class RabbitMqBus : IMessageBus
    {
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;

        public RabbitMqBus()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task PublishAsync<T>(
            string queue,
            T message)
        {
            _channel.QueueDeclare(queue, true, false, false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish("", queue, null, body);

            return Task.CompletedTask;
        }

        public void Subscribe<T>(
            string queue,
            Func<T, Task> handler)
        {
            _channel.QueueDeclare(queue, true, false, false);

            var consumer =
                new EventingBasicConsumer(_channel);

            consumer.Received += async (_, ea) =>
            {
                var json = Encoding.UTF8
                    .GetString(ea.Body.ToArray());

                var obj = JsonSerializer
                    .Deserialize<T>(json)!;

                await handler(obj);
            };

            _channel.BasicConsume(
                queue,
                true,
                consumer);
        }
    }
    }
