using FirstProject.Web.Models.Dto;

namespace FirstProject.Web.Services.Interfaces
{
    public interface IArticleService : IBaseService
    {
        Task<T> GetAllArticlesAsync<T>();
        Task<T> GetArticleByIdAsync<T>(int id);
        Task<T> CreateArticleAsync<T>(ArticleDto articleDto);
        Task<T> UpdateArticleAsync<T>(ArticleDto articleDto);
        Task<T> DeleteArticleAsync<T>(int id);
    }
}
