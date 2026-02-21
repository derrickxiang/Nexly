using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.Crawler.Services
{
    public class RssFetcher
    {
        public async Task<List<FeedItem>> FetchAsync(string url)
        {
            var feed = await FeedReader.ReadAsync(url);
            return feed.Items.ToList();
        }
    }
}
