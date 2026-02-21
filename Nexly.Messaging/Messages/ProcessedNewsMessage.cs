using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Messaging.Messages
{
    public class ProcessedNewsMessage
    {
        public string Url { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public string SummaryZh { get; set; }

        public float[] Embedding { get; set; }

        public string Category { get; set; }
    }

}
