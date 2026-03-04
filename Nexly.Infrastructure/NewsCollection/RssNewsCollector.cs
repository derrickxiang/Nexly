using Nexly.Application.Interfaces.Services;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Nexly.Infrastructure.NewsCollection;

public class RssNewsCollector : INewsCollector
{
    public async Task<List<CollectedNewsItem>> CollectAsync(
        string sourceUrl,
        CancellationToken ct)
    {
        using var reader = XmlReader.Create(sourceUrl);
        var feed = SyndicationFeed.Load(reader);

        return feed.Items.Select(item =>
            new CollectedNewsItem(
                item.Title.Text,
                item.Summary?.Text ?? "",
                item.Links.FirstOrDefault()?.Uri.ToString() ?? ""
            )).ToList();
    }
}