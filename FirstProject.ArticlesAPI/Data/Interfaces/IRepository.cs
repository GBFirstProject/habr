using System.Linq.Expressions;

namespace FirstProject.ArticlesAPI.Data.Interfaces
{
    public interface IRepository<TEntity>
    where TEntity : class
    {
        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] keys);

        void Delete(TEntity entity);        
    }
}
