using Nexly.Messaging.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Nexly.Messaging.Services;

public class RabbitMqBus : IMessageBus, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

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
        _channel.QueueDeclare(
            queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: body);

        return Task.CompletedTask;
    }

    public void Subscribe<T>(
        string queue,
        Func<T, Task> handler)
    {
        _channel.QueueDeclare(
            queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (_, ea) =>
        {
            var json = Encoding.UTF8
                .GetString(ea.Body.ToArray());

            var obj = JsonSerializer.Deserialize<T>(json);

            if (obj == null)
                return;

            try
            {
                await handler(obj);

                _channel.BasicAck(
                    ea.DeliveryTag,
                    false);
            }
            catch
            {
                _channel.BasicNack(
                    ea.DeliveryTag,
                    false,
                    true);
            }
        };

        _channel.BasicConsume(
            queue,
            autoAck: false,
            consumer: consumer);
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}