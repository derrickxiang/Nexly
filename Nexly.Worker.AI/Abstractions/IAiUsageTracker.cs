namespace Nexly.Worker.AI.Abstractions;

public interface IAiUsageTracker
{
    void Track(string provider, int inputTokens, int outputTokens);
}