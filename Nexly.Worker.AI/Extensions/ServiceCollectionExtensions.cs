using Nexly.Worker.AI.Services;
using RabbitMqBus = Nexly.Worker.AI.Services.RabbitMqBus;

namespace Nexly.Worker.AI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAI(this IServiceCollection services)
        {
            services.AddSingleton<IBus, RabbitMqBus>();
            services.AddHttpClient<IAiService, OpenAiService>();

            return services;
        }
    }
}
