using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.AI
{
    public class AIResult
    {
        public string Summary { get; set; } = default!;

        public string Category { get; set; } = default!;

        public double ImportanceScore { get; set; }

        public List<string> Companies { get; set; } = new();

        public List<string> Tags { get; set; } = new();
    }
}
