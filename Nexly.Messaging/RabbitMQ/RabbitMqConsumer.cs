using Microsoft.Extensions.DependencyInjection;
using Nexly.Messaging.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitMqConsumer<T, THandler> : EventingBasicConsumer
    where THandler : IMessageHandler<T>
{
    private readonly IServiceProvider _serviceProvider;

    public RabbitMqConsumer(
        IModel channel,
        IServiceProvider serviceProvider)
        : base(channel)
    {
        _serviceProvider = serviceProvider;

        Received += OnReceived;
    }

    private async void OnReceived(
        object? sender,
        BasicDeliverEventArgs args)
    {
        try
        {
            var body = args.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var message = JsonSerializer.Deserialize<T>(json);

            if (message is null)
            {
                Model.BasicNack(args.DeliveryTag, false, false);
                return;
            }

            using var scope = _serviceProvider.CreateScope();

            var handler = scope.ServiceProvider
                .GetRequiredService<THandler>();

            await handler.HandleAsync(
                message,
                CancellationToken.None);

            Model.BasicAck(args.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            Model.BasicNack(
                args.DeliveryTag,
                multiple: false,
                requeue: true);
        }
    }
}