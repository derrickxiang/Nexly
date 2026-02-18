namespace Nexly.Application.Interfaces;
public interface IArticleRepository
{
    Task<Article?> GetByIdAsync(Guid id);

    Task<List<Article>> GetLatestAsync(int count);

    Task<Article?> GetByHashAsync(string hash);

    Task AddAsync(Article article);

    Task SaveChangesAsync();

    IQueryable<Article> Query();
}