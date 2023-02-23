using FirstProject.ArticlesAPI.Data.Interfaces;
using FirstProject.ArticlesAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FirstProject.ArticlesAPI.Data
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly ArticlesDBContext _dbContext;
        private readonly DbSet<TEntity> _dbEntities;

        public Repository(ArticlesDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbEntities = _dbContext.Set<TEntity>();
        }
        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
        {
            var dbSet = _dbContext.Set<TEntity>();
            var query = includes
                .Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(dbSet, (current, include) => current.Include(include));

            return query ?? dbSet;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            CheckEntityForNull(entity);
            return (await _dbEntities.AddAsync(entity, cancellationToken)).Entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken) => await _dbEntities.AddRangeAsync(entities, cancellationToken);

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken) =>
            await Task.Run(() => _dbEntities.RemoveRange(entities), cancellationToken);

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken) =>
            await Task.Run(() => _dbEntities.Update(entity).Entity, cancellationToken);

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken) =>
            await Task.Run(() => _dbEntities.UpdateRange(entities), cancellationToken);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                if (_dbContext.Database.CurrentTransaction != null)
                {
                    await _dbContext.Database.CurrentTransaction.RollbackAsync(cancellationToken);
                }

                throw;
            }
        }

        public async ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] keys) =>
            await _dbEntities.FindAsync(keys, cancellationToken) ?? throw new DataNotFoundException(_dbEntities.EntityType.ToString(), keys.ToString());

        public void Delete(TEntity entity) => _dbEntities.Remove(entity);

        private static void CheckEntityForNull(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Запись для добавления не может быть null");
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
        }
    }
}
