using CodeHollow.FeedReader;

public class RssFetcher
{
    public async Task<List<RssItem>> FetchAsync(string url)
    {
        var feed = await FeedReader.ReadAsync(url);

        return feed.Items.Select(x => new RssItem
        {
            Title = x.Title,
            Link = x.Link,
            PublishDate = x.PublishingDate ?? DateTime.UtcNow,
            Content = x.Description
        }).ToList();
    }
}

public class RssItem
{
    public string Title { get; set; }
    public string Link { get; set; }
    public DateTime PublishDate { get; set; }
    public string Content { get; set; }
}
