using Nexly.Application.Interfaces;
using System.Linq.Expressions;

namespace Nexly.Application.Repositories
{
    public class RepositoryBase<T, TId> : IRepositoryBase<T>, IRepositoryBase2<T, TId> where T : class
    {
        public NexlyDbContext Context { get; set; }
        public RepositoryBase(NexlyDbContext NexlyDbContext)
        {
            Context = NexlyDbContext;
        }

        public void Create(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Context.Set<T>().AsEnumerable());
        }

        public Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(Context.Set<T>().Where(expression).AsEnumerable());
        }

        public async Task<bool> SaveAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<bool> IsExistAsync(TId id)
        {
            return await Context.Set<T>().FindAsync(id) != null;
        }

        public async Task<int> InsertAsync(IEnumerable<T> entities)
        {
            await Context.Set<T>().AddRangeAsync(entities);
            return await Context.SaveChangesAsync();
        }
    }
}
