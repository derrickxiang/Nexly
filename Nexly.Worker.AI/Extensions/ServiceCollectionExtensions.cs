using Nexly.Worker.AI.Abstractions;
using Nexly.Worker.AI.Infrastructure;
using Nexly.Worker.AI.Providers;
using Nexly.Worker.AI.Services;
using Nexly.Worker.AI.Workers;

namespace Nexly.Worker.AI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAI(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddSingleton<IAiUsageTracker, AiUsageTracker>();

       // services.AddHttpClient<OpenAiProvider>();
        services.AddHttpClient<SiliconFlowProvider>();

        //services.AddTransient<IAiProvider, OpenAiProvider>();
        services.AddTransient<IAiProvider, SiliconFlowProvider>();

        services.AddTransient<FallbackAiProvider>();

        services.AddTransient<IAiProvider>(sp =>
            sp.GetRequiredService<FallbackAiProvider>());

        services.AddTransient<IBus, RabbitMqBus>();
        services.AddTransient<AiService>();
        services.AddSingleton<RabbitMqConnection>();
        services.AddHostedService<AiWorker>();

        return services;
    }
}