using Nexly.Application.Articles.DTOs;
using Nexly.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexly.Application.Services
{
    public class ArticleService : IArticleService
    {

        private readonly IArticleRepository _repo;

    public ArticleService(IArticleRepository repo)
    {
        _repo = repo;
    }

        public Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Article> GetArticleByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Article> CreateArticleAsync(Article article)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateArticleAsync(Article article)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteArticleAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ArticleDto> GetLatestArticleAsync(int count)
        {
            throw new NotImplementedException();
        }

        public Task<ArticleDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddIfNotDuplicateAsync(ArticleDto dto)
        {
            var hash = ComputeHash(dto.Title + dto.Summary);

        var exist = await _repo.GetByHashAsync(hash);
        if (exist != null)
            return false;

        var entity = new Article
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            TitleZh = dto.TitleZh,
            Summary = dto.Summary,
            SummaryZh = dto.SummaryZh,
            SourceName = dto.SourceName,
            PublishedAt = dto.PublishedAt,
            CollectedAt = DateTime.UtcNow,
            Hash = hash,
            QualityScore = dto.QualityScore
        };

         _repo.Create(entity);
        await _repo.SaveAsync();

        return true;
        }

        private static string ComputeHash(string input)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
    }
}