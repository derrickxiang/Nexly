using Microsoft.Extensions.DependencyInjection;
using Nexly.Messaging.Abstractions;
using Nexly.Worker.Background;
using Nexly.Worker.Handlers;

namespace Nexly.Worker;

public static class WorkerDependencyInjection
{
    public static IServiceCollection AddWorkerServices(
        this IServiceCollection services)
    {
        services.AddScoped<ArticleCollectedHandler>();
        services.AddScoped<ArticleProcessedHandler>();

        services.AddHostedService<WorkerStartup>();

        return services;
    }
}