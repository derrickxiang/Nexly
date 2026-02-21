using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.Crawler.Models
{
    public class CrawlResult
    {
        public string Title { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string Source { get; set; } = default!;
        public DateTime PublishedAt { get; set; }

        public string? Content { get; set; }
        public string? Summary { get; set; }

        public string Language { get; set; } = "en";
    }
}
