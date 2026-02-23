using Nexly.Pipeline.News.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.Collectors
{
    public class CompanyNewsProvider : INewsProvider
    {
        private readonly HttpClient _http;

        public CompanyNewsProvider(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<RawNewsItem>> CollectAsync(
            CancellationToken cancellationToken = default)
        {
            var url = "https://www.intel.com/content/www/us/en/newsroom/home.html";

            var html = await _http.GetStringAsync(url, cancellationToken);

            // TODO: HTML parsing with AngleSharp / HtmlAgilityPack

            return Enumerable.Empty<RawNewsItem>();
        }
    }
}
