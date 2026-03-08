namespace Nexly.Domain.Models;

public record NewsItem(
    string Title,
    string Url,
    string Source,
    DateTime PublishedAt);