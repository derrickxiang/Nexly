using Nexly.Application.Common;
using Nexly.Application.Interfaces.Repositories;
using Nexly.Application.Interfaces.Services;
using Nexly.Domain.Enums;

namespace Nexly.Application.UseCases.ProcessArticle;

public class ProcessArticleHandler
{
    private readonly IArticleRepository _articleRepo;
    private readonly IAiProvider _aiProvider;

    public ProcessArticleHandler(
        IArticleRepository articleRepo,
        IAiProvider aiProvider)
    {
        _articleRepo = articleRepo;
        _aiProvider = aiProvider;
    }

    public async Task<Result<Guid>> Handle(
        Guid articleId,
        CancellationToken ct)
    {
        var article = await _articleRepo
            .GetByIdAsync(articleId, ct);

        if (article is null)
            return Result<Guid>.Failure("Article not found");

        try
        {
            var summary = await _aiProvider
                .GenerateSummaryAsync(article.Content, ct);

            article.MarkProcessed(summary);

            await _articleRepo.SaveChangesAsync(ct);

            return Result<Guid>.Success(article.Id);
        }
        catch
        {
            article.MarkFailed();
            await _articleRepo.SaveChangesAsync(ct);

            return Result<Guid>.Failure("AI processing failed");
        }
    }
}