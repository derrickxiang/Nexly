using Nexly.Application.Common;
using Nexly.Application.Interfaces.Repositories;
using Nexly.Application.Interfaces.Services;
using Nexly.Domain.Entities;

namespace Nexly.Application.UseCases.CollectNews;

public class CollectNewsHandler
{
    private readonly ISourceRepository _sourceRepo;
    private readonly IArticleRepository _articleRepo;
    private readonly INewsCollector _collector;

    public CollectNewsHandler(
        ISourceRepository sourceRepo,
        IArticleRepository articleRepo,
        INewsCollector collector)
    {
        _sourceRepo = sourceRepo;
        _articleRepo = articleRepo;
        _collector = collector;
    }

    public async Task<Result<int>> Handle(CancellationToken ct)
    {
        var sources = await _sourceRepo
            .GetActiveSourcesAsync(ct);

        int totalCollected = 0;

        foreach (var source in sources)
        {
            var items = await _collector
                .CollectAsync(source.BaseUrl, ct);

            foreach (var item in items)
            {
                var article = new Article(
                    item.Title,
                    item.Content,
                    item.Url,
                    source.Id);

                await _articleRepo.AddAsync(article, ct);

                totalCollected++;
            }
        }

        await _articleRepo.SaveChangesAsync(ct);

        return Result<int>.Success(totalCollected);
    }
}