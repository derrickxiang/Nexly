using Nexly.Application.DTOs;
using Nexly.Application.Interfaces.Repositories;
using Nexly.Domain.Enums;

namespace Nexly.Application.UseCases.GenerateDigest;

public class GenerateDigestHandler
{
    private readonly IArticleRepository _repo;

    public GenerateDigestHandler(IArticleRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ArticleDto>> Handle(
        CancellationToken ct)
    {
        var articles = await _repo
            .GetByStatusAsync(NewsStatus.Processed, 20, ct);

        return articles.Select(a =>
            new ArticleDto(
                a.Id,
                a.Title,
                a.AiSummary?.Summary ?? "",
                "Unknown",
                a.CreatedAt))
            .ToList();
    }
}