using Nexly.Domain.Entities;
using Nexly.Domain.Models;
using Nexly.Domain.Repositories;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Nexly.Infrastructure.News;

public class RssNewsProvider : INewsProvider
{
    private static readonly string[] Feeds =
    [
        "https://rss.nytimes.com/services/xml/rss/nyt/Technology.xml",
        "https://feeds.feedburner.com/TechCrunch",
        "https://www.theverge.com/rss/index.xml"
    ];

    public async Task<IEnumerable<Article>> FetchLatestAsync(
        CancellationToken ct)
    {
        var tasks = Feeds.Select(f => FetchFeedAsync(f, ct));

        var results = await Task.WhenAll(tasks);

        return results
            .SelectMany(x => x)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(50);
    }

    private async Task<IEnumerable<Article>> FetchFeedAsync(
        string url,
        CancellationToken ct)
    {
        try
        {
            using var reader = XmlReader.Create(url);

            var feed = SyndicationFeed.Load(reader);

            if (feed == null)
                return Enumerable.Empty<Article>();

            return feed.Items.Select(item =>
                new Article(
                    item.Title.Text,
                    item.Links.FirstOrDefault()?.Uri.ToString() ?? "",
                    feed.Title.Text,
                    new Guid()
                ));
        }
        catch
        {
            return Enumerable.Empty<Article>();
        }
    }
}