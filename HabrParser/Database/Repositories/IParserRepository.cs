using HabrParser.Models.APIArticles;

namespace HabrParser.Database.Repositories
{
    public interface IParserRepository : IDisposable
    {
        Task<Article> CreateHabrArticle(Article article, ArticleThreadLevelType threadLevel = ArticleThreadLevelType.None, CancellationToken cancellationToken = default);
        Task<int> LastArticleId(ArticleThreadLevelType levelType, CancellationToken cancellationToken);
        Task<Article> ArticleAlreadyExists(int hubrId, CancellationToken cancellationToken);
    }
}
