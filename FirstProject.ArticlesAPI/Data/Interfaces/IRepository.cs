using System.Linq.Expressions;

namespace FirstProject.ArticlesAPI.Data.Interfaces
{
    public interface IRepository<TEntity>
    {                
        void Create(TEntity data);
        void Update(TEntity data);
        void Delete(TEntity data);
        void Edit(TEntity data);
        IQueryable<TEntity> FindAll();
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
