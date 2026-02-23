using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.Collectors
{
    using Nexly.Pipeline.News.Models;
    using System.ServiceModel.Syndication;
    using System.Xml;

    public class RssNewsProvider : INewsProvider
    {
        private readonly HttpClient _http;

        private readonly string[] _feeds =
        {
        "https://news.google.com/rss/search?q=semiconductor",
        "https://www.eetimes.com/feed/",
        "https://semiengineering.com/feed/"
    };

        public RssNewsProvider(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<RawNewsItem>> CollectAsync(
            CancellationToken cancellationToken = default)
        {
            var list = new List<RawNewsItem>();

            foreach (var feed in _feeds)
            {
                using var stream = await _http.GetStreamAsync(feed, cancellationToken);

                using var reader = XmlReader.Create(stream);

                var rss = SyndicationFeed.Load(reader);

                foreach (var item in rss.Items)
                {
                    var raw = new RawNewsItem
                    {
                        Title = item.Title.Text,
                        Url = item.Links.FirstOrDefault()?.Uri.ToString() ?? "",
                        Source = rss.Title.Text,
                        PublishedAt = item.PublishDate.UtcDateTime,
                        Content = item.Summary?.Text ?? ""
                    };

                    list.Add(raw);
                }
            }

            return list;
        }
    }
}
