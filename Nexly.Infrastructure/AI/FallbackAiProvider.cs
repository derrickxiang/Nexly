using Nexly.Application.Interfaces.Services;
using Nexly.Domain.ValueObjects;

namespace Nexly.Infrastructure.AI;

public class FallbackAiProvider : IAiProvider
{
    private readonly IEnumerable<IAiProvider> _providers;

    public FallbackAiProvider(IEnumerable<IAiProvider> providers)
    {
        _providers = providers;
    }

    public async Task<AiSummary> GenerateSummaryAsync(
        string content,
        CancellationToken ct)
    {
        foreach (var provider in _providers)
        {
            try
            {
                return await provider
                    .GenerateSummaryAsync(content, ct);
            }
            catch
            {
                // try next
            }
        }

        throw new Exception("All AI providers failed");
    }
}