using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Nexly.Worker.AI.Services
{
    public class RabbitMqBus : IBus, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public RabbitMqBus()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
        }

        public void Subscribe(string queue, Func<string, Task> handler)
        {
            _channel.QueueDeclareAsync(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null).Wait();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await handler(message);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            _channel.BasicConsumeAsync(queue, false, consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
