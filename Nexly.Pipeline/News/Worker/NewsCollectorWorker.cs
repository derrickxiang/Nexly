using Nexly.Pipeline.News.Collectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.Worker
{
    public class NewsCollectorWorker : BackgroundService
    {
        private readonly INewsCollector _collector;
        private readonly NewsPipeline _pipeline;
        private readonly ILogger<NewsCollectorWorker> _logger;

        public NewsCollectorWorker(
            INewsCollector collector,
            NewsPipeline pipeline,
            ILogger<NewsCollectorWorker> logger)
        {
            _collector = collector;
            _pipeline = pipeline;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var items = await _collector.CollectAsync();

                foreach (var item in items)
                {
                    try
                    {
                        await _pipeline.ProcessAsync(item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "News processing failed");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
