using Nexly.Application.UseCases.ProcessArticle;
using Nexly.Messaging.Abstractions;
using Nexly.Messaging.Contracts;

namespace Nexly.Worker.Handlers;

public class ArticleCollectedHandler
    : IMessageHandler<ArticleCollectedEvent>
{
    private readonly ProcessArticleHandler _handler;

    public ArticleCollectedHandler(
        ProcessArticleHandler handler)
    {
        _handler = handler;
    }

    public async Task HandleAsync(
        ArticleCollectedEvent message,
        CancellationToken ct)
    {
        await _handler.Handle(
            message.ArticleId,
            ct);
    }
}