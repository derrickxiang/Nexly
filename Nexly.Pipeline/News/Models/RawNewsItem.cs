using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.Models
{
    public class RawNewsItem
    {
        public string Title { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string Source { get; set; } = default!;
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; } = default!;
    }
}
