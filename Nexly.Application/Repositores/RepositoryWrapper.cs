using Nexly.Application.Interfaces;
using Nexly.Application.Repositores;

namespace Nexly.Application.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IArticleRepository _article = null;

        private INewsRepository _news = null;
       
        public RepositoryWrapper(NexlyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IArticleRepository Article => _article ?? new ArticleRepository(dbContext);
        public INewsRepository News => _news ?? new NewsRepository(dbContext);

        public NexlyDbContext dbContext { get; }
    }
}
