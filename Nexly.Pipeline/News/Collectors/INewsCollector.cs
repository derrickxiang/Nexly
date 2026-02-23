using Nexly.Pipeline.News.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.Collectors
{
    public interface INewsCollector
    {
        Task<IEnumerable<RawNewsItem>> CollectAsync(
            CancellationToken cancellationToken = default);
    }
}
