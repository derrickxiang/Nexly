using System.Collections.Generic;
using System.Threading.Tasks;
using Nexly.Application.Articles.DTOs;

namespace Nexly.Application.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleDto> GetLatestArticleAsync(int count);
        Task<ArticleDto> GetByIdAsync(int id);
        Task<bool> AddIfNotDuplicateAsync(ArticleDto article);
    }
}