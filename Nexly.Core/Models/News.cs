using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Domain
{
    public class News
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string Summary { get; set; } = default!;

        public string Content { get; set; } = default!;

        public string Source { get; set; } = default!;

        public string Url { get; set; } = default!;

        public string Category { get; set; } = default!;

        public double ImportanceScore { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<NewsCompany> Companies { get; set; } = new();

        public List<NewsTag> Tags { get; set; } = new();
    }
}
