using Nexly.Pipeline.News.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace Nexly.Pipeline.News.Collectors
{
    public class GoogleNewsProvider : INewsProvider
    {
        private readonly HttpClient _http;

        private readonly string[] _keywords =
        {
        "semiconductor",
        "TSMC",
        "ASML",
        "chip manufacturing",
        "Nvidia AI chip"
    };

        public GoogleNewsProvider(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<RawNewsItem>> CollectAsync(
            CancellationToken cancellationToken = default)
        {
            var list = new List<RawNewsItem>();

            foreach (var keyword in _keywords)
            {
                var url =
                    $"https://news.google.com/rss/search?q={Uri.EscapeDataString(keyword)}&hl=en-US&gl=US&ceid=US:en";

                using var stream = await _http.GetStreamAsync(url, cancellationToken);

                using var reader = XmlReader.Create(stream);

                var feed = SyndicationFeed.Load(reader);

                foreach (var item in feed.Items)
                {
                    list.Add(new RawNewsItem
                    {
                        Title = item.Title.Text,
                        Url = item.Links.FirstOrDefault()?.Uri.ToString() ?? "",
                        Source = "Google News",
                        PublishedAt = item.PublishDate.UtcDateTime,
                        Content = item.Summary?.Text ?? ""
                    });
                }
            }

            return list;
        }
    }
}
