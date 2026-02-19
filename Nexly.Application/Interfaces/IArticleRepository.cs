namespace Nexly.Application.Interfaces;
public interface IArticleRepository : IRepositoryBase<Article>, IRepositoryBase2<Article, Guid>
{
    Task<Article?> GetByHashAsync(string hash);
}