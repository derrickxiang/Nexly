using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexly.Application.Interfaces.Repositories;
using Nexly.Application.Interfaces.Services;
using Nexly.Infrastructure.AI;
using Nexly.Infrastructure.NewsCollection;
using Nexly.Infrastructure.Persistence;
using Nexly.Infrastructure.Persistence.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection
        AddInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
    {
        services.AddDbContext<NexlyDbContext>(options =>
            options.UseSqlite(
                config.GetConnectionString("Default")));

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ISourceRepository, SourceRepository>();

        services.AddScoped<IAiProvider, OpenAiProvider>();
        services.AddScoped<IClock, SystemClock>();
        services.AddScoped<INewsCollector, RssNewsCollector>();

        return services;
    }
}