using FirstProject.ArticleAPI.Models.Dto;

namespace FirstProject.ArticleAPI.Repository
{
    public interface IArticleRepository
    {
        Task<IEnumerable<ArticleDto>> GetArticles();
        Task<ArticleDto> GetArticleById(int articleId);
        Task<ArticleDto> CreateUpdateArticle(ArticleDto articleDto);
        Task<bool> DeleteArticle(int articleId);
    }
}
