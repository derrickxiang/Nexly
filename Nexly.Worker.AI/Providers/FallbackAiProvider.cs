using Nexly.Worker.AI.Abstractions;

namespace Nexly.Worker.AI.Providers;

public class FallbackAiProvider : IAiProvider
{
    private readonly IEnumerable<IAiProvider> _providers;
    private readonly ILogger<FallbackAiProvider> _logger;

    public string Name => "Fallback";

    public FallbackAiProvider(
        IEnumerable<IAiProvider> providers,
        ILogger<FallbackAiProvider> logger)
    {
        _providers = providers;
        _logger = logger;
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
    {
        foreach (var provider in _providers.Where(p => p.Name != "Fallback"))
        {
            try
            {
                _logger.LogInformation("Trying provider: {Provider}", provider.Name);

                return await provider.GenerateAsync(prompt, ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Provider {Provider} failed, trying next...",
                    provider.Name);
            }
        }

        throw new Exception("All AI providers failed.");
    }
}