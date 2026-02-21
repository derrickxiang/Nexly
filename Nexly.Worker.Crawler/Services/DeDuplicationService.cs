using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Worker.Crawler.Services
{
    public class DeduplicationService
    {
        private readonly HashSet<string> _cache = new();

        public bool IsDuplicate(string url)
        {
            var hash = url.GetHashCode().ToString();

            if (_cache.Contains(hash))
                return true;

            _cache.Add(hash);
            return false;
        }
    }
}
