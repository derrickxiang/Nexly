using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nexly.Application.Articles.DTOs;
using Nexly.Application.Interfaces;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceProvider sp, ILogger<Worker> logger)
    {
        _sp = sp;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await RunPipeline();

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private async Task RunPipeline()
    {
        using var scope = _sp.CreateScope();

        var ArticleService =
            scope.ServiceProvider.GetRequiredService<IArticleService>();

        var fetcher = new RssFetcher();

        var items = await fetcher.FetchAsync(
            "https://feeds.bbci.co.uk/Article/rss.xml");

        foreach (var item in items)
        {
            try
            {
                var dto = new ArticleDto
                {
                    Title = item.Title,
                    Summary = item.Content,
                    SourceName = "BBC",
                    PublishedAt = item.PublishDate,
                    QualityScore = 0.8f
                };

                await ArticleService.AddIfNotDuplicateAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pipeline error");
            }
        }
    }
}
