using HabrParser.Database;
using HabrParser.Models.APIArticles;

namespace HabrParser.Interfaces
{
    public interface IArticlesRepository : IDisposable
    {
        Task<Article> CreateHabrArticle(Article article, ArticleThreadLevelType threadLevel = ArticleThreadLevelType.None, CancellationToken cancellationToken = default);
        Task<int> LastArticleId(ArticleThreadLevelType levelType, CancellationToken cancellationToken);
        Task<Article> ArticleAlreadyExists(int hubrId, CancellationToken cancellationToken);

        Task<Author> CreateAuthor(Author author, CancellationToken cancellationToken);
    }
}
