using Nexly.Application.Core;

namespace Nexly.Application.Articles.Queries
{
    public class ArticleParams : PaginationParams<DateTime?>
    {
         public string? Filter { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}