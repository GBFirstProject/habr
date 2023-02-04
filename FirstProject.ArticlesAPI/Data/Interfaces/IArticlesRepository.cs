using FirstProject.ArticlesAPI.Models;

namespace FirstProject.ArticlesAPI.Data.Interfaces
{
    public interface IArticlesRepository : IRepository<Article>
    {
        Task<Article> GetArticleById(Guid id, CancellationToken cancellationToken);
    }
}
