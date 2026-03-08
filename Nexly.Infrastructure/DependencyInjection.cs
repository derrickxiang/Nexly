using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexly.Application.Interfaces.Repositories;
using Nexly.Application.Interfaces.Services;
using Nexly.Domain.Repositories;
using Nexly.Infrastructure.AI;
using Nexly.Infrastructure.News;
using Nexly.Infrastructure.NewsCollection;
using Nexly.Infrastructure.Persistence;
using Nexly.Infrastructure.Persistence.Repositories;
using Nexly.Infrastructure.Services;

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
        //services.AddScoped<INewsCollector, RssNewsCollector>();
        services.AddScoped<INewsProvider, RssNewsProvider>();

        return services;
    }
}