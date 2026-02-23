using Microsoft.EntityFrameworkCore;
using Nexly.Application.Interfaces;
using Nexly.Application.Repositories;
using Nexly.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Nexly.Application.Repositores
{
    public class NewsRepository : RepositoryBase<News, Guid>, INewsRepository
    {
        public NewsRepository(NexlyDbContext context) : base(context)
        {
        }

        public Task<bool> ExistsByHashAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public new async Task<IEnumerable<News>> GetByConditionAsync(Expression<Func<News, bool>> expression)
        {
            return await Context.Set<News>()
                    .Where(expression)
                    .ToListAsync();
        }

    }
}
