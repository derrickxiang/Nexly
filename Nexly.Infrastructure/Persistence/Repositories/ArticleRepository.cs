using Microsoft.EntityFrameworkCore;
using Nexly.Application.Interfaces.Repositories;
using Nexly.Domain.Entities;
using Nexly.Domain.Enums;

namespace Nexly.Infrastructure.Persistence.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly NexlyDbContext _context;

    public ArticleRepository(NexlyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Article article, CancellationToken ct)
    {
        await _context.Articles.AddAsync(article, ct);
    }

    public async Task<Article?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Articles
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Article>> GetByStatusAsync(
        NewsStatus status,
        int take,
        CancellationToken ct)
    {
        return await _context.Articles
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsByUrlAsync(string url, CancellationToken ct)
    {
        return await _context.Articles
            .AnyAsync(x => x.Url == url, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}