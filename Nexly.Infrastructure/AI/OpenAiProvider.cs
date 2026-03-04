using Nexly.Application.Interfaces.Services;
using Nexly.Domain.ValueObjects;

namespace Nexly.Infrastructure.AI;

public class OpenAiProvider : IAiProvider
{
    public async Task<AiSummary> GenerateSummaryAsync(
        string content,
        CancellationToken ct)
    {
        // TODO: 调用真实 OpenAI API

        await Task.Delay(500, ct);

        var usage = new TokenUsage(100, 200, 0.02m);

        return new AiSummary(
            summary: content.Substring(0, Math.Min(200, content.Length)),
            model: "gpt-4o-mini",
            usage: usage);
    }
}