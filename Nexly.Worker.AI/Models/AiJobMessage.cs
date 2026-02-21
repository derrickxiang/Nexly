using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.AI.Models
{
    public class AiJobMessage
    {
        public Guid NewsId { get; set; }
        public string Content { get; set; } = "";
    }
}
