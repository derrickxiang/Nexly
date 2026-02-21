using Nexly.Worker.Crawler.Messaging;
using Nexly.Worker.Crawler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.Crawler.Services
{
    public class FeedCrawlerService
    {
        private readonly RssFetcher _rss;
        private readonly ArticleParser _parser;
        private readonly DeduplicationService _dedup;
        private readonly EventPublisher _publisher;

        public FeedCrawlerService(
            RssFetcher rss,
            ArticleParser parser,
            DeduplicationService dedup,
            EventPublisher publisher)
        {
            _rss = rss;
            _parser = parser;
            _dedup = dedup;
            _publisher = publisher;
        }

        public async Task CrawlAsync(string feedUrl, string source)
        {
            var items = await _rss.FetchAsync(feedUrl);

            foreach (var item in items)
            {
                if (_dedup.IsDuplicate(item.Link))
                    continue;

                var content = await _parser.ParseContentAsync(item.Link);

                var result = new CrawlResult
                {
                    Title = item.Title,
                    Url = item.Link,
                    Source = source,
                    PublishedAt = item.PublishingDate ?? DateTime.UtcNow,
                    Content = content
                };

                _publisher.Publish(result);
            }
        }
    }
}
