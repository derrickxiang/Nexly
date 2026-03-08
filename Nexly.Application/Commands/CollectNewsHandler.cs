using MediatR;
using Nexly.Application.Interfaces.Messaging;
using Nexly.Application.Interfaces.Repositories;
using Nexly.Domain.Entities;
using Nexly.Domain.Repositories;

namespace Nexly.Application.Commands;

public class CollectNewsHandler
    : IRequestHandler<CollectNewsCommand, int>
{
    private readonly INewsProvider _provider;
    private readonly IArticleRepository _repository;
    private readonly IMessageBus _bus;

    public CollectNewsHandler(
        INewsProvider provider,
        IArticleRepository repository,
        IMessageBus bus)
    {
        _provider = provider;
        _repository = repository;
        _bus = bus;
    }

    public async Task<int> Handle(
        CollectNewsCommand request,
        CancellationToken ct)
    {
        var articles = await _provider.FetchLatestAsync(ct);

        int count = 0;

        foreach (var item in articles)
        {
            var article = new Article(
                item.Title,
                item.Content,
                item.Url,
                item.SourceId
            );

            await _repository.AddAsync(article, ct);

            await _bus.PublishAsync(
                new Messaging.Contracts.ArticleProcessedEvent(article.Id),
                ct);

            count++;
        }

        return count;
    }
}