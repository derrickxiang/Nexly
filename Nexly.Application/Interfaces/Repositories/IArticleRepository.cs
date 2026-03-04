using Nexly.Domain.Entities;

namespace Nexly.Application.Interfaces.Repositories;

public interface IArticleRepository
{
    Task AddAsync(Article article, CancellationToken ct);

    Task<Article?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<bool> ExistsByUrlAsync(string url, CancellationToken ct);

    Task<List<Article>> GetByStatusAsync(
        Domain.Enums.NewsStatus status,
        int take,
        CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);
}