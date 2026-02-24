using System;
using System.Collections.Generic;
using System.Text;

namespace Nexly.Pipeline.News.AI
{
    public interface IAIClient
    {
        Task<string> GenerateAsync(
            string prompt,
            CancellationToken cancellationToken = default);

        Task<T> GenerateJsonAsync<T>(
            string prompt,
            CancellationToken cancellationToken = default);
    }
}
