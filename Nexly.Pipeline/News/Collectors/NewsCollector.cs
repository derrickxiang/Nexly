using Nexly.Pipeline.News.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.Collectors
{
    public class NewsCollector : INewsCollector
    {
        private readonly IEnumerable<INewsCollector> _providers;
        private readonly ILogger<NewsCollector> _logger;

        public NewsCollector(
            IEnumerable<INewsCollector> providers,
            ILogger<NewsCollector> logger)
        {
            _providers = providers;
            _logger = logger;
        }

        public async Task<IEnumerable<RawNewsItem>> CollectAsync(
            CancellationToken cancellationToken = default)
        {
            var tasks = _providers
                .Select(p => SafeCollect(p, cancellationToken));

            var results = await Task.WhenAll(tasks);

            return results.SelectMany(x => x);
        }

        private async Task<IEnumerable<RawNewsItem>> SafeCollect(
            INewsCollector provider,
            CancellationToken token)
        {
            try
            {
                return await provider.CollectAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Provider failed: {Provider}", provider.GetType().Name);
                return Enumerable.Empty<RawNewsItem>();
            }
        }
    }
}
