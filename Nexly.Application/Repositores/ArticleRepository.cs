using Microsoft.EntityFrameworkCore;
using Nexly.Application.Interfaces;
using System.Linq.Expressions;

namespace Nexly.Application.Repositories
{
    public class ArticleRepository : RepositoryBase<Article, Guid>, IArticleRepository
    {
        public ArticleRepository(NexlyDbContext context) : base(context)
        {
        }

        public new async Task<IEnumerable<Article>> GetByConditionAsync(Expression<Func<Article, bool>> expression)
        {
            return await Context.Set<Article>()
                    .Where(expression)
                    .ToListAsync();
        }
    }
}
