using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.Enums;
using FirstProject.ArticlesAPI.Models.Requests;

namespace FirstProject.ArticlesAPI.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<Article> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<Article> CreateArticleAsync(CreateArticleRequest articleDto, CancellationToken cancellationToken);
        public Task<Article> UpdateArticleDataAsync(UpdateArticleDataDto updateArticleDataDto, CancellationToken cancellationToken);        
        public Task<bool> DeleteArticleAsync(Guid id, CancellationToken cancellationToken);        
    }
}
