namespace Nexly.Application.Interfaces.Services;

public interface INewsCollector
{
    Task<List<CollectedNewsItem>> CollectAsync(
        string sourceUrl,
        CancellationToken ct);
}

public record CollectedNewsItem(
    string Title,
    string Content,
    string Url);