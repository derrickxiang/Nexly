using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Messaging.Messages
{
    public class RawNewsMessage
    {
        public string Source { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
    }

}
