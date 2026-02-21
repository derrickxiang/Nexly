using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.Contracts
{
    public class AiRequestMessage
    {
        public Guid ArticleId { get; set; }

        public string Title { get; set; } = default!;

        public string Content { get; set; } = default!;

        public string Category { get; set; } = default!;

        public string Source { get; set; } = default!;

        public DateTime PublishedAt { get; set; }
    }
}
