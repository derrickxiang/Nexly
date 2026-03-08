using Nexly.Messaging.Abstractions;
using Nexly.Messaging.Contracts;

namespace Nexly.Worker.Handlers;

public class ArticleProcessedHandler
    : IMessageHandler<ArticleProcessedEvent>
{
    public Task HandleAsync(
        ArticleProcessedEvent message,
        CancellationToken ct)
    {
        // TODO: 处理后续逻辑
        return Task.CompletedTask;
    }
}