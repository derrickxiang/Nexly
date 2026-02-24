using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.AI
{
    public class AIClientOptions
    {
        public string ApiKey { get; set; } = default!;

        public string Model { get; set; } = "gpt-4.1-mini";

        public string BaseUrl { get; set; } = "https://api.openai.com/v1/";

        public int TimeoutSeconds { get; set; } = 60;
    }
}
