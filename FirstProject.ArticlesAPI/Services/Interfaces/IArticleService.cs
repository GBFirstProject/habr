using FirstProject.ArticlesAPI.Models;
using FirstProject.ArticlesAPI.Models.DTO;
using FirstProject.ArticlesAPI.Models.Enums;
using FirstProject.ArticlesAPI.Models.Requests;
using System.Collections;

namespace FirstProject.ArticlesAPI.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<List<PreviewArticleDTO>> GetPreviewArticles(PagingParameters paging, CancellationToken cancellationToken);
        public Task<FullArticleDTO> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<PreviewArticleDTO> GetPreviewArticleByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<Guid> CreateArticleAsync(CreateArticleRequest articleDto, CancellationToken cancellationToken);
        public Task<FullArticleDTO> UpdateArticleDataAsync(UpdateArticleRequest updateArticleDataDto, Guid userId, CancellationToken cancellationToken);        
        public Task<bool> DeleteArticleAsync(Guid id, Guid userId, CancellationToken cancellationToken);        
        public int GetArticlesCount(CancellationToken cancellationToken);
        public Task<SearchPreviewResultDTO> GetPreviewArticleByHubLastMonthAsync(string hub, PagingParameters paging, CancellationToken cancellationToken);
        public Task<SearchPreviewResultDTO> GetPreviewArticleByTagLastMonthAsync(string tag, PagingParameters paging, CancellationToken cancellationToken);
        public Task<List<ArticlesByAuthorDTO>> GetArticlesTitlesByAuthorId(Guid authorId, CancellationToken cancellationToken);
        public Task<FullArticleDTO> LikeArticle(Guid articleId, Guid userId, CancellationToken cts);
        public Task<FullArticleDTO> DislikeArticle(Guid articleId, Guid userId, CancellationToken cts);
    }
}
