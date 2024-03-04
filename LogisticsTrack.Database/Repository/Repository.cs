using LogisticsTrack.Domain.BaseEntities;
using System.Linq.Expressions;

namespace LogisticsTrack.Database.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        public LogisticsContext Context { get; }

        public Repository(LogisticsContext context)
        {
            Context = context;
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public virtual TEntity GetEntity(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            TEntity result = null;
            if (filter != null)
            {
                result = query.FirstOrDefault(filter);
            }
            return result;
        }
         
        public void Delete(TEntity item)
        {
            Context.Set<TEntity>().Remove(item); // we handle the soft delete in the context
        }

        public TEntity Add(TEntity item)
        {
            Context.Set<TEntity>().Add(item);
            return item;
        }

        public void AddMany(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
