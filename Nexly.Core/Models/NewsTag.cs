using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Domain
{
    public class NewsTag
    {
        public Guid Id { get; set; }

        public Guid NewsId { get; set; }

        public string Tag { get; set; } = default!;
    }
}
