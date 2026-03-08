using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexly.Messaging.Abstractions;
using Nexly.Messaging.RabbitMQ;

namespace Nexly.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(
            configuration.GetSection("RabbitMq"));

        services.AddSingleton<RabbitMqConnectionManager>();
        services.AddSingleton<IMessageBus, RabbitMqBus>();

        return services;
    }
}