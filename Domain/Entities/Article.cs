using Nexly.Domain.Base;
using Nexly.Domain.Enums;
using Nexly.Domain.ValueObjects;

namespace Nexly.Domain.Entities;

public class Article : Entity
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public string Url { get; private set; }

    public Guid SourceId { get; private set; }

    public NewsStatus Status { get; private set; } = NewsStatus.Raw;

    public AiSummary? AiSummary { get; private set; }

    public int RetryCount { get; private set; }

    private Article() { } // For ORM

    public Article(string title, string content, string url, Guid sourceId)
    {
        Title = title;
        Content = content;
        Url = url;
        SourceId = sourceId;
    }

    public void MarkProcessed(AiSummary summary)
    {
        AiSummary = summary;
        Status = NewsStatus.Processed;
        MarkUpdated();
    }

    public void MarkFailed()
    {
        RetryCount++;
        Status = NewsStatus.Failed;
        MarkUpdated();
    }

    public void ResetToRaw()
    {
        Status = NewsStatus.Raw;
        MarkUpdated();
    }
}
