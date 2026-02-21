using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.Services
{
    public class CategoryService
    {
        public string Detect(string title)
        {
            title = title.ToLower();

            if (title.Contains("ai") || title.Contains("tech"))
                return "Technology";

            if (title.Contains("singapore"))
                return "Singapore";

            if (title.Contains("market") || title.Contains("stock"))
                return "Business";

            return "General";
        }
    }
}
