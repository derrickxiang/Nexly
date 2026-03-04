using Nexly.Domain.ValueObjects;

namespace Nexly.Application.Interfaces.Services;

public interface IAiProvider
{
    Task<AiSummary> GenerateSummaryAsync(
        string content,
        CancellationToken ct);
}