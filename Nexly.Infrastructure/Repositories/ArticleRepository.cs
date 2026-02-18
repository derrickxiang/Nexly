using Nexly.Domain.Entities;
using Nexly.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexly.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NexlyDbContext _db;

    public ArticleRepository(NexlyDbContext db)
    {
        _db = db;
    }

    public async Task<Article?> GetByIdAsync(Guid id)
    {
        return await _db.Articles
            .Include(x => x.Topics)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Article>> GetLatestAsync(int count)
    {
        return await _db.Articles
            .OrderByDescending(x => x.PublishedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Article?> GetByHashAsync(string hash)
    {
        return await _db.Articles
            .FirstOrDefaultAsync(x => x.Hash == hash);
    }

    public async Task AddAsync(Article article)
    {
        await _db.Articles.AddAsync(article);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public IQueryable<Article> Query()
    {
        return _db.Articles.AsQueryable();
    }
    }
}