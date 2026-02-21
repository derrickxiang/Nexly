using Nexly.Worker.Crawler.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.Crawler.Workers
{
    public class CrawlWorker : BackgroundService
    {
        private readonly FeedCrawlerService _crawler;

        public CrawlWorker(FeedCrawlerService crawler)
        {
            _crawler = crawler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _crawler.CrawlAsync(
                    "https://www.channelnewsasia.com/rssfeeds/8395986",
                    "CNA");

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}
