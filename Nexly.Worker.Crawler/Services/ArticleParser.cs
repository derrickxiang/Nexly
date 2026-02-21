using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.Crawler.Services
{
    public class ArticleParser
    {
        private readonly HttpClient _http;

        public ArticleParser(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> ParseContentAsync(string url)
        {
            var html = await _http.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var paragraphs = doc.DocumentNode
                .SelectNodes("//p")
                ?.Select(p => p.InnerText);

            return paragraphs == null ? null : string.Join("\n", paragraphs);
        }
    }
}
