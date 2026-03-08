using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nexly.Messaging.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Nexly.Messaging.RabbitMQ;

public class RabbitMqBus : IMessageBus
{
    private readonly RabbitMqConnectionManager _connectionManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMqOptions _options;

    public RabbitMqBus(
        RabbitMqConnectionManager connectionManager,
        IServiceProvider serviceProvider,
        IOptions<RabbitMqOptions> options)
    {
        _connectionManager = connectionManager;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    // =========================
    // Publish
    // =========================
    public Task PublishAsync<T>(
        T message,
        CancellationToken ct = default)
    {
        using var channel = _connectionManager.CreateChannel();

        channel.ExchangeDeclare(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(message));

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true; // 持久化消息

        var routingKey = typeof(T).Name;

        channel.BasicPublish(
            exchange: _options.ExchangeName,
            routingKey: routingKey,
            basicProperties: properties,
            body: body);

        return Task.CompletedTask;
    }

    // =========================
    // Subscribe
    // =========================
    public void Subscribe<T, THandler>()
        where THandler : IMessageHandler<T>
    {
        var channel = _connectionManager.CreateChannel();

        channel.ExchangeDeclare(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        var queueName = $"{typeof(T).Name}.queue";

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        channel.QueueBind(
            queue: queueName,
            exchange: _options.ExchangeName,
            routingKey: typeof(T).Name);

        channel.BasicQos(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (sender, args) =>
        {
            try
            {
                var body = args.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var message = JsonSerializer.Deserialize<T>(json);

                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider
                    .GetRequiredService<THandler>();

                await handler.HandleAsync(
                    message!,
                    CancellationToken.None);

                channel.BasicAck(
                    deliveryTag: args.DeliveryTag,
                    multiple: false);
            }
            catch (Exception ex)
            {
                // TODO: 日志记录

                channel.BasicNack(
                    deliveryTag: args.DeliveryTag,
                    multiple: false,
                    requeue: false); // 可改成 true 做重试
            }
        };

        channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumer);
    }
}