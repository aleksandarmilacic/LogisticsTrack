
using LogisticsTrack.Domain.BaseEntities;
using System.Linq.Expressions;

namespace LogisticsTrack.Database.Repository
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        LogisticsContext Context { get; }

        TEntity Add(TEntity item);
        void AddMany(IEnumerable<TEntity> entities);
        void Delete(TEntity item);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);
        TEntity GetEntity(Expression<Func<TEntity, bool>> filter = null);
        Task<int> SaveAsync();
    }
}