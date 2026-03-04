using Microsoft.Extensions.DependencyInjection;
using Nexly.Messaging.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Nexly.Messaging.RabbitMQ;

public class RabbitMqConsumer<T, THandler>
    : EventingBasicConsumer
    where THandler : IMessageHandler<T>
{
    private readonly IServiceProvider _serviceProvider;

    public RabbitMqConsumer(
        IModel channel,
        IServiceProvider serviceProvider)
        : base(channel)
    {
        _serviceProvider = serviceProvider;

        Received += async (sender, args) =>
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

            Channel.BasicAck(
                args.DeliveryTag,
                multiple: false);
        };
    }
}