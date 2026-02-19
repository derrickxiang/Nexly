using Nexly.Application.Interfaces;

namespace Nexly.Application.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IArticleRepository _article = null;

        public RepositoryWrapper(NexlyDbContext dbContext)
        {
            dbContext = dbContext;
        }

        public IArticleRepository Article => _article ?? new ArticleRepository(dbContext);

        public NexlyDbContext dbContext { get; }
    }
}
