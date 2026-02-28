using Nexly.Worker.AI.Abstractions;

namespace Nexly.Worker.AI.Services;

public class AiService
{
    private readonly IAiProvider _provider;

    public AiService(IAiProvider provider)
    {
        _provider = provider;
    }

    public Task<string> GenerateAsync(string prompt, CancellationToken ct)
        => _provider.GenerateAsync(prompt,ct);
}