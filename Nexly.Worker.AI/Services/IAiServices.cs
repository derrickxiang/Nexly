using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.AI.Services
{
    public interface IAiService
    {
        Task<AiResult> ProcessAsync(string content);
    }

    public class AiResult
    {
        public string Summary { get; set; } = "";
        public string Chinese { get; set; } = "";
    }
}
