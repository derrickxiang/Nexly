using Nexly.Worker.Crawler.Messaging;
using Nexly.Worker.Crawler.Services;
using Nexly.Worker.Crawler.Workers;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Http;

namespace Nexly.Worker.Crawler.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCrawler(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddSingleton<RssFetcher>();
            services.AddSingleton<ArticleParser>();
            services.AddSingleton<DeduplicationService>();
            services.AddSingleton<EventPublisher>();

            services.AddSingleton<FeedCrawlerService>();

            services.AddHostedService<CrawlWorker>();

            return services;
        }
    }
}
